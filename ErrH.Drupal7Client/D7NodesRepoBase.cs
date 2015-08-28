using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.Drupal7Models;
using ErrH.Tools.ErrorConstructors;
using ErrH.Tools.Extensions;

namespace ErrH.Drupal7Client
{
    public abstract class D7NodesRepoBase<TNodeDto, TClass> : ListRepoBase<TClass>
    {
        const int RETRY_INTERVAL_SEC = 5;

        private event EventHandler _afterPreload;

        private ID7Client _client;



        public D7NodesRepoBase(ID7Client d7Client)
        {
            _client = d7Client;
        }


        public override bool Load(params object[] args)
        {
            if (args == null || args.Length != 2)
                Throw.BadArg(nameof(args), "should have 2 items");
            var rsrc = args[0].ToString().Slash(args[1]);


            if (!_client.IsLoggedIn) return Error_n(
                "Currently disconnected from data source.",
                    "Call Connect() before Load().");

            _afterPreload += async (s, e) 
                => { await TryAndTry(rsrc); };

            _afterPreload?.Invoke(this, EventArgs.Empty);
            return true;
        }


        private async Task TryAndTry(string resourceUrl)
        {
            _afterPreload = null;
            var cancelSrc = new CancellationTokenSource();
            Cancelled += (s, e) => { cancelSrc.Cancel(); };

            while (!cancelSrc.Token.IsCancellationRequested)
            {
                if (await DoActualLoad(resourceUrl)) return;

                Warn_n("Failed to load remote data.", 
                      $"Retrying after {RETRY_INTERVAL_SEC} seconds...");

                await DelayRetry(RETRY_INTERVAL_SEC);
            }
        }


        private async Task DelayRetry(int seconds)
        {
            FireRetrying(seconds);
            await TaskEx.Delay(1000 * seconds);
        }



        private async Task<bool> DoActualLoad(string rsrc)
        {
            Fire_Loading();

            Debug_n("Loading repository data from source...", rsrc);

            var dtos = await _client.Get<List<TNodeDto>>(rsrc, null,
                        "Successfully loaded repository data ({0} fetched).",
                            x => "{0:record}".f(x.Count));

            if (dtos == null) return false;

            _list = dtos.Select(x => FromDto(x)).ToList();
            Fire_Loaded();
            return true;
        }


        protected override List<TClass> LoadList(object[] args)
        {
            throw Error.BadAct("In an implementation of ISlowRepository<T>, method LoadList() should not be called.");
        }


        protected abstract TClass FromDto(TNodeDto dto);
    }
}
