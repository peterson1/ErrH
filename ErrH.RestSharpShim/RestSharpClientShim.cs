using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ErrH.Tools.ErrorConstructors;
using ErrH.Tools.Extensions;
using ErrH.Tools.Loggers;
using ErrH.Tools.RestServiceShim;
using ErrH.Tools.RestServiceShim.RestExceptions;
using Newtonsoft.Json;
using RestSharp.Portable;

namespace ErrH.RestSharpShim
{
    public class RestSharpClientShim : LogSourceBase, IClientShim
    {

        public string BaseUrl { get; set; }



        public async Task<T> Send<T>(IRequestShim request,
                                     CancellationToken cancelToken,
                                     string taskIntro,
                                     string successMessage,
                                     params Func<T, object>[] successMsgArgs
                                     )
        {
            var client = CreateClient();
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

            if (resp == null) return default(T);

            var args = successMsgArgs.Select(x =>
                        x.Invoke(resp.Data)).ToArray();

            Trace_o(successMessage.IsBlank() ? "response: [{0}] {1}".f((int)resp.StatusCode, resp.StatusDescription.Quotify()) : successMessage.f(args));
            return resp.Data;
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

            return new ResponseShim(resp, request, this.BaseUrl, err);
        }


        private RestClient CreateClient()
        {
            Throw.IfBlank(BaseUrl, nameof(BaseUrl));
            //return new RestClient(this.BaseUrl);
            return new RestClient(new Uri(this.BaseUrl));
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



        private T Unhandled<T>(T ex) where T : Exception
        {
            Error_o("Request failed. (unhandled)");
            Fatal_n("Unhandled Exception", ex.Details());
            return ex;
        }
    }
}
