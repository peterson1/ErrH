using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ErrH.Tools.Drupal7Models.DTOs;
using ErrH.Tools.Drupal7Models.Entities;
using ErrH.Tools.Extensions;
using ErrH.Tools.ScalarEventArgs;
using Newtonsoft.Json;
using NLog;
using Polly;
using ServiceStack;

namespace ErrH.WpfRestClient.net45.DataAccess
{
    public class D7Client
    {
        private static Logger _logr = LogManager.GetCurrentClassLogger();

        protected string _baseURL;
        protected string _userName;
        protected string _password;
        protected int    _userID;
        protected string _currentJson;

        private       EventHandler<EArg<D7NodeBase>> _postCompleted;
        public  event EventHandler<EArg<D7NodeBase>>  PostCompleted
        {
            add    { _postCompleted -= value; _postCompleted += value; }
            remove { _postCompleted -= value; }
        }

        private       EventHandler<EArg<D7NodeBase>> _putCompleted;
        public  event EventHandler<EArg<D7NodeBase>>  PutCompleted
        {
            add    { _putCompleted -= value; _putCompleted += value; }
            remove { _putCompleted -= value; }
        }

        private      EventHandler _attemptFailed;
        public event EventHandler  AttemptFailed
        {
            add    { _attemptFailed -= value; _attemptFailed += value; }
            remove { _attemptFailed -= value; }
        }


        public D7Client(string baseUrl, string userName, string password, int userID = 0)
        {
            _baseURL  = baseUrl;
            _userName = userName;
            _password = password;
            _userID   = userID;
        }


        public Task<List<T>> List<T>(string resource) 
            => PersistentGet<List<T>>(resource);


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
                _postCompleted?.Invoke(this, new EArg<D7NodeBase> { Value = node });
            });
        }


        public async void PersistentPut<T>(D7NodeMap<T> objToPut)
            where T : D7NodeBase, new()
        {
            await PersistentPolicy<T>("PUT").ExecuteAsync(async () =>
            {
                var node = await Put(objToPut, _userID);
                _putCompleted?.Invoke(this, new EArg<D7NodeBase> { Value = node });
            });
        }


        private Policy PersistentPolicy<T>(string httpMethod, int delaySeconds = 4)
        {
            var typ = typeof(T).Name;

            var policy = Policy.Handle<Exception>().RetryForeverAsync(ex =>
            {
                _attemptFailed?.Invoke(this, EventArgs.Empty);
                LogThenWait(httpMethod, typ, ex, delaySeconds);
            });

            return policy;
        }



        protected virtual async Task<T> Get<T>(string resource)
        {
            var d7c = CreateClient();
            return await d7c.GetAsync<T>(resource);
        }


        private async Task<D7NodeBase> Post<T>(D7NodeMap<T> objToPost, int userID)
            where T : D7NodeBase, new()
        {
            var d7c = CreateClient();
            var dto = objToPost.ToNodeDTO(userID);

            _currentJson = JsonConvert.SerializeObject(dto, Formatting.Indented);

            //return await d7c.PostAsync<D7NodeBase>("/entity_node/", dto);
            return await d7c.PostAsync<D7NodeBase>("/entity_node/", _currentJson);
        }


        private async Task<D7NodeBase> Put<T>(D7NodeMap<T> objToPost, int userID)
            where T : D7NodeBase, new()
        {
            var d7c  = CreateClient();
            var dto  = objToPost.ToNodeDTO(userID) as ID7NodeRevision;
            dto.vid  = objToPost.vid;
            var rsrc = $"/entity_node/{dto.nid}";
            _currentJson = JsonConvert.SerializeObject(dto, Formatting.Indented);
            _logr.Trace(rsrc + L.f + _currentJson);
            return await d7c.PutAsync<D7NodeBase>(rsrc, _currentJson);
        }


        private void LogThenWait<T>(string httpMethod, string typeName, T ex, int seconds) where T : Exception
        {
            var titl = $"{httpMethod} ‹{typeName}› failed.";
            var msg = $"{titl}  :  ‹{ex.GetType().Name}› {ex.Details(false, false)}";
            _logr.Error(msg);

            //if (httpMethod == "POST") _logr.Trace(_currentJson);

            switch (httpMethod)
            {
                case "POST":
                case "PUT":
                    _logr.Trace(_currentJson);
                    break;
            }

            Thread.Sleep(1000 * seconds);
        }


        protected JsonServiceClient CreateClient() 
            => new JsonServiceClient(_baseURL)
            {
                UserName = _userName,
                Password = _password,
                AlwaysSendBasicAuthHeader = true,
            };
    }
}
