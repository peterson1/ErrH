using System;
using System.Threading;
using System.Threading.Tasks;
using ErrH.RestClient.PCL45.Drupal7Models;
using ErrH.RestClient.PCL45.EventArguments;
using ErrH.RestClient.PCL45.Extensions;
using Newtonsoft.Json;
using Polly;
using ServiceStack;

namespace ErrH.RestClient.PCL45.Writers
{
    public class D7BasicAuthWriter : IPersistentWriter
    {
        protected string _baseURL;
        protected string _userName;
        protected string _password;
        protected int    _userID;

        private      EventHandler<EArg<string>> _attemptFailed;
        public event EventHandler<EArg<string>>  AttemptFailed
        {
            add    { _attemptFailed -= value; _attemptFailed += value; }
            remove { _attemptFailed -= value; }
        }

        //private      EventHandler<EArg<ID7NodeRevision>> _posted;
        //public event EventHandler<EArg<ID7NodeRevision>>  Posted
        //{
        //    add    { _posted -= value; _posted += value; }
        //    remove { _posted -= value; }
        //}

        //private      EventHandler<EArg<ID7NodeRevision>> _putted;
        //public event EventHandler<EArg<ID7NodeRevision>>  Putted
        //{
        //    add    { _putted -= value; _putted += value; }
        //    remove { _putted -= value; }
        //}

        //private      EventHandler _cancelled;
        //public event EventHandler  Cancelled
        //{
        //    add    { _cancelled -= value; _cancelled += value; }
        //    remove { _cancelled -= value; }
        //}



        public D7BasicAuthWriter(string baseUrl, string userName, int userID, string password)
        {
            _baseURL  = baseUrl;
            _userName = userName;
            _userID   = userID;
            _password = password;
        }


        public async Task<ID7PutDTO> Post<T>(T d7Node, 
            CancellationToken cancelr, int delaySeconds = 4)
            where T : ID7PostDTO
        {
            return await PersistentPolicy<T>("POST", d7Node, delaySeconds)
                .ExecuteAsync(async ct =>
            {
                if (ct.IsCancellationRequested) return null;
                var d7c = CreateClient();
                var url = "/entity_node/";
                return await d7c.PostAsync<ID7PutDTO>(url, d7Node).ConfigureAwait(false);
            }
            , cancelr).ConfigureAwait(false);
        }


        private Policy PersistentPolicy<T>(string httpMethod, ID7PostDTO d7Node, int delaySeconds)
        {
            var typ = typeof(T).Name;

            var policy = Policy.Handle<Exception>().RetryForeverAsync(ex =>
            {
                var msg = GetMessage(httpMethod, d7Node, typ, ex);
                _attemptFailed?.Invoke(this, new EArg<string> { Value = msg });

                using (EventWaitHandle tmpEvent = new ManualResetEvent(false))
                {
                    tmpEvent.WaitOne(TimeSpan.FromSeconds(delaySeconds));
                }
            });
            return policy;
        }


        private string GetMessage<TEx>(string httpMethod, ID7PostDTO d7Node, string typ, TEx ex)
            where TEx : Exception
        {
            var titl = $"Error: {typeof(TEx).FullName}" + L.f
                     + $"[{httpMethod}] ‹{typ}› failed on {_baseURL}";

            var json = JsonConvert.SerializeObject(d7Node, Formatting.Indented);

            return titl + L.f 
                + $"‹{ex.GetType().Name}› {ex.Details(false, false)}" + L.f
                + json;
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
