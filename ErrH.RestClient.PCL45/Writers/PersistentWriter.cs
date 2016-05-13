using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ErrH.RestClient.PCL45.EventArguments;
using ErrH.RestClient.PCL45.Extensions;
using ErrH.RestClient.PCL45.Policies;
using Newtonsoft.Json;
using Polly;
using ServiceStack;
using ServiceStack.Text;

namespace ErrH.RestClient.PCL45.Writers
{
    public class PersistentWriter : IPersistentWriter
    {
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


        protected string _userName;
        protected string _password;
        protected string _baseURL;


        public int RetryDelaySeconds { get; set; } = 4;


        public PersistentWriter(string userName, string password, string baseURL)
        {
            _userName = userName;
            _password = password;
            _baseURL  = baseURL;
        }


        public async Task<T> Post<T>(T d7Node, CancellationToken cancelr, string resource = "/entity_node/")
        {
            //return await PersistentPolicy<T>("POST", d7Node)
            return await OnCrappyWeb.ExecuteAsync(async ct =>
            {
                if (ct.IsCancellationRequested) return default(T);
                var d7c = CreateClient();
                return await d7c.PostAsync<T>(resource, d7Node).ConfigureAwait(false);
            }
            , cancelr).ConfigureAwait(false);
        }



        public async Task<T> Put<T>(T d7Node, int nid,
            CancellationToken cancelr, string resource = "/entity_node/{0}")
        {
            //return await PersistentPolicy<T>("PUT", d7Node)
            return await OnCrappyWeb.ExecuteAsync(async ct =>
                {
                    if (ct.IsCancellationRequested) return default(T);
                    var d7c = CreateClient();
                    var url = string.Format(resource, nid);

                    //var js = JsonConvert.SerializeObject(d7Node, Formatting.Indented);

                    return await d7c.PutAsync<T>(url, d7Node).ConfigureAwait(false);

                    //var js = await d7c.PutAsync<string>(url, d7Node).ConfigureAwait(false);
                    //var obj = JsonConvert.DeserializeObject<T>(js);
                    //return obj;
                }
            , cancelr).ConfigureAwait(false);
        }



        public async Task<bool> Delete(int nid, CancellationToken cancelr, string resource = "/entity_node/")
        {
            //return await PersistentPolicy<object>("DELETE", null)
            return await OnCrappyWeb.ExecuteAsync(async ct =>
            {
                if (ct.IsCancellationRequested) return false;
                var url = resource + nid;
                var d7c = CreateClient();
                object resp;
                try {
                    resp = await d7c.DeleteAsync<object>(url).ConfigureAwait(false);
                }
                catch (Exception ex){ throw ex; }
                return true;
            }
            , cancelr).ConfigureAwait(false);
        }



        private Policy OnCrappyWeb
            => OnCrappyConnection.RetryForever(RetryDelaySeconds,
                x => _attemptFailed?.Invoke(this, new EArg<string>(x)));




        //private Policy PersistentPolicy<T>(string httpMethod, T d7Node)
        //{
        //    var typ = typeof(T).Name;

        //    var policy = Policy.Handle<Exception>().RetryForeverAsync(ex =>
        //    {
        //        var msg = GetMessage(httpMethod, d7Node, typ, ex);
        //        _attemptFailed?.Invoke(this, new EArg<string> { Value = msg });

        //        using (EventWaitHandle tmpEvent = new ManualResetEvent(false))
        //        {
        //            tmpEvent.WaitOne(TimeSpan.FromSeconds(RetryDelaySeconds));
        //        }
        //    });
        //    return policy;
        //}



        //private string GetMessage<T, TEx>(string httpMethod, T d7Node, string typ, TEx ex)
        //    where TEx : Exception
        //{
        //    var titl = $"Error: {typeof(TEx).FullName}" + L.f
        //             + $"[{httpMethod}] ‹{typ}› failed on {_baseURL}";

        //    var json = JsonConvert.SerializeObject(d7Node, Formatting.Indented);

        //    return titl + L.f 
        //        + $"‹{ex.GetType().Name}› {ex.Details(false, false)}" + L.f
        //        + json;
        //}


        protected JsonServiceClient CreateClient()
        {
            JsConfig.TreatEnumAsInteger = true;
            return new JsonServiceClient(_baseURL)
            {
                UserName = _userName,
                Password = _password,
                AlwaysSendBasicAuthHeader = true,
            };
        }
    }
}
