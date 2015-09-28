using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ErrH.Tools.Authentication;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.Drupal7Models;
using ErrH.Tools.ErrorConstructors;
using ErrH.Tools.Extensions;

namespace ErrH.Drupal7Client
{
    public abstract class D7NodesRepoBase<TNodeDto, TClass> : ListRepoBase<TClass>
    {
        const int RETRY_INTERVAL_SEC = 5;

        protected ID7Client      _client;
        private LoginCredentials _credentials;



        public override void SetClient(ISessionClient sessionClient, LoginCredentials credentials)
        {
            _client = ForwardLogs(sessionClient.As<ID7Client>());
            _credentials = credentials;
        }



        //public override bool Add(TClass itemToAdd)
        //{
        //    var valid = base.Add(itemToAdd);
        //    if (valid) _newItems.Add(itemToAdd);
        //    return valid;
        //}



        public override async Task<bool> LoadAsync(params object[] args)
        {
            if (args == null || args.Length != 2)
                Throw.BadArg(nameof(args), "should have 2 items");
            var rsrc = args[0].ToString().Slash(args[1]);


            //if (!_client.IsLoggedIn) return Error_n(
            //    "Currently disconnected from data source.",
            //        "Call Connect() before Load().");                

            var cancelSrc = new CancellationTokenSource();
            Cancelled += (s, e) => { cancelSrc.Cancel(); };

            return await TryAndTry(rsrc, cancelSrc.Token);
        }


        private async Task<bool> TryAndTry
            (string resourceUrl, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (await DoActualLoad(resourceUrl, token)) return true;

                Warn_n("Failed to load remote data.", 
                      $"Retrying after {RETRY_INTERVAL_SEC} seconds...");

                await DelayRetry(RETRY_INTERVAL_SEC, token);
            }
            return false;
        }


        private async Task DelayRetry
            (int seconds, CancellationToken token)
        {
            for (int i = seconds; i > 0; i--)
            {
                RaiseDelayingRetry(i);

                try   { await TaskEx.Delay(1000, token); }
                catch { return; }
            }
        }



        private async Task<bool> DoActualLoad(string rsrc, CancellationToken cancelToken)
        {
            RaiseLoading();

            Debug_n("Loading repository data from source...", rsrc);

            if (!_client.IsLoggedIn)
                if (_client.HasSavedSession) _client.LoadSession();

            if (!_client.IsLoggedIn)
                if (!await _client.Login(_credentials, cancelToken)) return false;


            var dtos = await _client.Get<List<TNodeDto>>(rsrc, cancelToken, null,
                        "Successfully loaded repository data ({0} fetched).",
                            x => "{0:record}".f(x.Count));

            if (dtos == null) return false;

            _list = dtos.Select(x => FromDto(x)).ToList();

            if (_list.Count == 1 && _list[0] == null) _list.Clear();

            RaiseLoaded();
            return true;
        }


        protected override List<TClass> LoadList(object[] args)
        {
            throw Error.BadAct("Implementations of D7NodesRepoBase<T> should call LoadAsync() instead of Load().");
        }


        protected abstract TClass FromDto(TNodeDto dto);
    }
}
