using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ErrH.Tools.Drupal7Models.DTOs;
using ErrH.Tools.Drupal7Models.Entities;
using ErrH.Tools.Extensions;
using ErrH.Tools.ScalarEventArgs;
using ErrH.Wpf.net45.BinUpdater;
using NLog;
using Polly;
using ServiceStack;

namespace ErrH.WpfRestClient.net45.DataAccess
{
    class D7Client
    {
        private static Logger _logr = LogManager.GetCurrentClassLogger();

        private string _baseURL;
        private string _userName;
        private string _password;
        private int    _userID;

        public event EventHandler<EArg<D7NodeBase>> Posted;
        public event EventHandler AttemptFailed;


        public D7Client(string baseUrl, string userName, string password, int userID = 0)
        {
            _baseURL  = baseUrl;
            _userName = userName;
            _password = password;
            _userID   = userID;
        }

        public async Task<T> PersistentGet<T>(string resource)
        {
            return await PersistentPolicy<T>("GET").ExecuteAsync(async () =>
            {
                return await Get<T>(resource);
            });
        }



        public async void PersistentPost<T>(D7NodeMap<T> objToPost)
            where T : D7NodeBase, new()
        {
            await PersistentPolicy<T>("POST").ExecuteAsync(async () =>
            {
                var node = await Post(objToPost, _userID);
                Posted?.Invoke(this, new EArg<D7NodeBase> { Value = node });
            });
        }


        private Policy PersistentPolicy<T>(string httpMethod, int delaySeconds = 4)
        {
            var typ = typeof(T).Name;

            var policy = Policy.Handle<Exception>().RetryForeverAsync(ex =>
            {
                AttemptFailed?.Invoke(this, EventArgs.Empty);
                LogThenWait(httpMethod, typ, ex, delaySeconds);
            });

            return policy;
        }



        private async Task<T> Get<T>(string resource)
        {
            var d7c = CreateClient();
            return await d7c.GetAsync<T>(resource);
        }


        private async Task<D7NodeBase> Post<T>(D7NodeMap<T> objToPost, int userID)
            where T : D7NodeBase, new()
        {
            var d7c = CreateClient();
            var dto = objToPost.ToNodeDTO(userID);
            return await d7c.PostAsync<D7NodeBase>("/entity_node/", dto);
        }


        private void LogThenWait<T>(string httpMethod, string typeName, T ex, int seconds) where T : Exception
        {
            var titl = $"{httpMethod} ‹{typeName}› failed.";
            var msg = $"{titl}  :  ‹{ex.GetType().Name}› {ex.Details(false, false)}";
            _logr.Error(msg);
            Thread.Sleep(1000 * seconds);
        }


        private JsonServiceClient CreateClient() 
            => new JsonServiceClient(_baseURL)
            {
                UserName = _userName,
                Password = _password,
                AlwaysSendBasicAuthHeader = true,
            };
    }
}
