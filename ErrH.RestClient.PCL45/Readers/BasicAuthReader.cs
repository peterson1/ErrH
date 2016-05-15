using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ErrH.RestClient.PCL45.EventArguments;
using ErrH.RestClient.PCL45.Extensions;
using ErrH.RestClient.PCL45.Policies;
using Polly;
using ServiceStack;

namespace ErrH.RestClient.PCL45.Readers
{
    public class BasicAuthReader
    {
        protected string _baseURL;
        protected string _userName;
        protected string _password;

        protected    EventHandler<EArg<string>> _attemptFailed;
        public event EventHandler<EArg<string>>  AttemptFailed
        {
            add    { _attemptFailed -= value; _attemptFailed += value; }
            remove { _attemptFailed -= value; }
        }


        public BasicAuthReader(string baseUrl, string userName, string password)
        {
            _baseURL  = baseUrl;
            _userName = userName;
            _password = password;
        }


        public Task<List<T>> List<T>(string resource)
            => PersistentGet<List<T>>(resource);


        public async Task<T> PersistentGet<T>(string resource)
        {
            //return await PersistentPolicy<T>(resource)
            return await OnCrappyWeb.ExecuteAsync(async () =>
            {
                return await Get<T>(resource).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }


        private Policy OnCrappyWeb
            => OnCrappyConnection.RetryForever(4,
                x => _attemptFailed?.Invoke(this, new EArg<string>(x)));


        //private Policy PersistentPolicy<T>(string resource, int delaySeconds = 4)
        //{
        //    var typ = typeof(T).Name;

        //    var policy = Policy.Handle<Exception>().RetryForeverAsync(ex =>
        //    {
        //        var msg = GetMessage(resource, typ, ex);
        //        _attemptFailed?.Invoke(this, new EArg<string> { Value = msg });
        //        //Task.Delay(1000 * delaySeconds);

        //        using (EventWaitHandle tmpEvent = new ManualResetEvent(false))
        //        {
        //            tmpEvent.WaitOne(TimeSpan.FromSeconds(delaySeconds));
        //        }

        //    });

        //    return policy;
        //}


        private string GetMessage<T>(string resource, string typ, T ex)
            where T : Exception
        {
            var titl = $"[GET] {resource} ‹{typeof(T).FullName}› failed on {_baseURL}";
            return $"{titl}{L.f}‹{ex.GetType().Name}› {ex.Details(false, false)}";
        }


        protected virtual async Task<T> Get<T>(string resource)
        {
            var d7c = CreateClient();
            return await d7c.GetAsync<T>(resource).ConfigureAwait(false);
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
