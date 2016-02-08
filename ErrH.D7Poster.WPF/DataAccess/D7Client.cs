using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ErrH.D7Poster.WPF.Configuration;
using ErrH.Tools.Drupal7Models.DTOs;
using ErrH.Tools.Drupal7Models.Entities;
using ErrH.Tools.Extensions;
using ErrH.Tools.Randomizers;
using ErrH.Tools.ScalarEventArgs;
using ErrH.Wpf.net45.BinUpdater;
using NLog;
using Polly;
using ServiceStack;

namespace ErrH.D7Poster.WPF.DataAccess
{
    class D7Client
    {
        private static Logger _logr     = LogManager.GetCurrentClassLogger();

        private static SettingsCfg _cfg = SettingsCfg.Load<SettingsCfg>();

        public event EventHandler<EArg<D7NodeBase>> Posted;
        public event EventHandler AttemptFailed;


        internal async Task<T> PersistentGet<T>(string resource, BinUpdaterKey cfg = null)
        {
            return await PersistentPolicy<T>("GET").ExecuteAsync(async () =>
            {
                return await Get<T>(resource, cfg);
            });
        }



        internal async void PersistentPost<T>(D7NodeMap<T> objToPost)
            where T : D7NodeBase, new()
        {
            await PersistentPolicy<T>("POST").ExecuteAsync(async () =>
            {
                var node = await Post(objToPost);
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



        private async Task<T> Get<T>(string resource, BinUpdaterKey cfg = null)
        {
            var d7c = CreateClient(cfg?.Username, cfg?.Password, cfg?.BaseURL);
            return await d7c.GetAsync<T>(resource);
        }


        private async Task<D7NodeBase> Post<T>(D7NodeMap<T> objToPost)
            where T : D7NodeBase, new()
        {
            var d7c = CreateClient();
            var dto = objToPost.ToNodeDTO(_cfg.UserID);
            return await d7c.PostAsync<D7NodeBase>("/entity_node/", dto);
        }


        private void LogThenWait<T>(string httpMethod, string typeName, T ex, int seconds) where T : Exception
        {
            var titl = $"{httpMethod} ‹{typeName}› failed.";
            var msg = $"{titl}  :  ‹{ex.GetType().Name}› {ex.Details(false, false)}";
            _logr.Error(msg);
            Thread.Sleep(1000 * seconds);
        }


        private static JsonServiceClient CreateClient
            (string userName = null, string password = null, string baseURL = null)
                => new JsonServiceClient(baseURL ?? _cfg.BaseURL)
                {
                    UserName = userName ?? _cfg.Username,
                    Password = password ?? _cfg.Password,
                    AlwaysSendBasicAuthHeader = true,
                };
    }
}
