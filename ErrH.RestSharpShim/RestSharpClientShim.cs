using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ErrH.Tools.Drupal7Models.Entities;
using ErrH.Tools.ErrorConstructors;
using ErrH.Tools.Extensions;
using ErrH.Tools.Loggers;
using ErrH.Tools.RestServiceShim;
using ErrH.Tools.RestServiceShim.RestExceptions;
using ErrH.Tools.ScalarEventArgs;
using Newtonsoft.Json;
using RestSharp.Portable;

namespace ErrH.RestSharpShim
{
    public class RestSharpClientShim : LogSourceBase, IClientShim
    {
        public event EventHandler<EArg<bool>> ResponseReceived;

        public string BaseUrl { get; set; }


        public async Task<T> Send<T>(IRequestShim request,
                                     CancellationToken cancelToken,
                                     string taskIntro,
                                     string successMessage,
                                     params Func<T, object>[] successMsgArgs
                                     )
        {
            var client = CreateClient();
            if (client == null) return default(T);

            var req = request as RequestShim;
            bool tryNoParse = false;

            Trace_i(taskIntro.IsBlank() ? "  [{0}] {1} ...".f(req.Method.ToString().ToUpper(), req.Resource) : taskIntro);

            IRestResponse<T> resp = null; try
            {
                resp = await client.Execute<T>(req.UnShim(), cancelToken);
            }
            catch (HttpRequestException ex)
            {
                var err = RestErr(ex, req);
                if (err is InvalidSslRestException)
                    Warn_h("Server is using a self-signed certificate.",
                           "Application must be set to allow SSL from the server.");
                throw err;
            }
            catch (JsonSerializationException ex)
            { tryNoParse = ParseErr<T>(ex); }
            catch (JsonReaderException ex)
            { tryNoParse = ParseErr<T>(ex); }
            catch (Exception ex) { throw Unhandled(ex); }
            finally { client.Dispose(); }


            if (tryNoParse) await TryUnserialized<T>(req, cancelToken);

            if (resp == null)
                return Warn_(default(T), "resp == null", "Unexpected NULL response from Send<>");

            var args = successMsgArgs.Select(x =>
                        x.Invoke(resp.Data)).ToArray();

            var msg = successMessage.IsBlank()
                    ? $"response ‹{typeof(T).Name}›" 
                    + $": [{(int)resp.StatusCode}]" 
                    + $" “{resp.StatusDescription}”"
                    : successMessage.f(args);
            Trace_o(msg);

            RaiseResponseReceived(true);
            return resp.Data;
        }


        public async Task<bool> Send<T>(CancellationToken tkn, List<IRequestShim> list) where T : D7NodeBase, new()
        {
            var rc = CreateClient();
            var job = new List<Task<IRestResponse<T>>>();

            foreach (RequestShim req in list)
                job.Add(rc.Execute<T>(req.UnShim(), tkn));

            return await RunBy(10, job);
        }


        private async Task<bool> RunBy<T>(int pageSize, List<Task<IRestResponse<T>>> job) where T : D7NodeBase, new()
        {
            if (job.Count <= pageSize)
                return await RunAll(job);

            var runs = (job.Count / pageSize) + 1;
            for (int i = 1; i < runs; i++)
            {
                var batch = job.Page(i, pageSize);
                if (!await RunAll<T>(batch)) return false;
            }
            return true;
        }


        private async Task<bool> RunAll<T>(IEnumerable<Task<IRestResponse<T>>> job) where T : D7NodeBase, new()
        {
            IRestResponse<T>[] ok; try
            {
                ok = await TaskEx.WhenAll(job);
            }
            catch (Exception ex) { return LogError("TaskEx.WhenAll(job)", ex); }
            return ok.All(x => x.IsSuccess);
        }



        public async Task<IResponseShim> Send(IRequestShim request,
                                              CancellationToken cancelToken,
                                              string taskIntro, 
                                              object successMessage, 
                                              params object[] successMsgArgs)
        {
            var client = CreateClient();
            var req = request as RequestShim;

            Trace_i(taskIntro.IsBlank() ? "  [{0}] {1} ...".f(req.Method.ToString().ToUpper(), req.Resource) : taskIntro);

            RestServiceException err = null;
            IRestResponse resp = null; try
            {
                resp = await client.Execute(req.UnShim(), cancelToken);
            }
            catch (HttpRequestException ex)
            { err = RestErr(ex, req); }
            catch (Exception ex) { throw Unhandled(ex); }
            finally { client.Dispose(); }

            if (resp != null)
                Trace_o((successMessage == null) ? "response: [{0}] {1}".f((int)resp.StatusCode, resp.StatusDescription.Quotify()) : successMessage.ToString().f(successMsgArgs));

            RaiseResponseReceived(true);
            return new ResponseShim(resp, request, this.BaseUrl, err);
        }


        private RestClient CreateClient()
        {
            Throw.IfBlank(BaseUrl, nameof(BaseUrl));
            try {
                return new RestClient(new Uri(this.BaseUrl));
            }
            catch (Exception ex)
            {
                LogError("new RestClient", ex);
                return null;
            }
        }


        private bool ParseErr<T>(Exception ex)
        {
            Error_n($"‹{typeof(T).Name}› Json parse error.", $"‹{typeof(T).FullName}›" + L.f + ex.Details());
            return true;
        }

        private async Task TryUnserialized<T>(IRequestShim req, CancellationToken cancelToken)
        {
            Warn_o("Parsing error.");

            //var expctd = JsonConvert.SerializeObject(new T(), Formatting.Indented);
            //Trace_n("Inspecting unserialized...", "expected format:" + L.f + expctd);

            var resp = await ((IClientShim)this).Send(req, cancelToken);

            if (resp != null)
                Trace_n("Inspecting unserialized...", "actual from server:" + L.f + resp.Content);
        }


        private RestServiceException RestErr(HttpRequestException ex, RequestShim req)
        {
            Error_o("HTTP request failed.");
            return RestError.Parse(ex, this, req);
        }


        protected void RaiseResponseReceived(bool isSuccess)
            => ResponseReceived?.Invoke(this, new EArg<bool> { Value = isSuccess });


        private T Unhandled<T>(T ex) where T : Exception
        {
            Error_o("Request failed. (unhandled)");
            Fatal_n("Unhandled Exception", ex.Details());
            return ex;
        }
    }
}
