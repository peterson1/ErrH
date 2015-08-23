using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ErrH.Tools.Extensions;
using ErrH.Tools.Loggers;
using ErrH.Tools.RestServiceShim;
using Newtonsoft.Json;
using RestSharp.Portable;

namespace ErrH.RestSharpShim
{
    public class RestSharpClientShim : LogSourceBase, IClientShim
    {

        public string BaseUrl { get; set; }



        public async Task<T> Send<T>(IRequestShim request,
                                     string taskIntro,
                                     string successMessage,
                                     params Func<T, object>[] successMsgArgs
                                     ) where T : new()
        {
            var client = new RestClient(this.BaseUrl);
            var req = request as RequestShim;
            bool tryNoParse = false;

            Trace_i(taskIntro.IsBlank() ? "  [{0}] {1} ...".f(req.Method.ToString().ToUpper(), req.Resource) : taskIntro);

            IRestResponse<T> resp = null; try
            {
                resp = await client.Execute<T>(req.UnShim);
            }
            catch (HttpRequestException ex)
            { throw RestErr(ex, req); }
            catch (JsonSerializationException ex)
            { tryNoParse = ParseErr(ex); }
            catch (JsonReaderException ex)
            { tryNoParse = ParseErr(ex); }
            catch (Exception ex) { throw Unhandled(ex); }
            finally { client.Dispose(); }


            if (tryNoParse) await TryUnserialized<T>(req);

            if (resp == null) return default(T);

            var args = successMsgArgs.Select(x =>
                        x.Invoke(resp.Data)).ToArray();

            Trace_o(successMessage.IsBlank() ? "response: [{0}] {1}".f((int)resp.StatusCode, resp.StatusDescription.Quotify()) : successMessage.f(args));
            return resp.Data;
        }



        public async Task<IResponseShim> Send(IRequestShim request, string taskIntro, object successMessage, params object[] successMsgArgs)
        {
            var client = new RestClient(this.BaseUrl);
            var req = request as RequestShim;

            Trace_i(taskIntro.IsBlank() ? "  [{0}] {1} ...".f(req.Method.ToString().ToUpper(), req.Resource) : taskIntro);

            RestServiceException err = null;
            IRestResponse resp = null; try
            {
                resp = await client.Execute(req.UnShim);
            }
            catch (HttpRequestException ex)
            { err = RestErr(ex, req); }
            catch (Exception ex) { throw Unhandled(ex); }
            finally { client.Dispose(); }

            if (resp != null)
                Trace_o((successMessage == null) ? "response: [{0}] {1}".f((int)resp.StatusCode, resp.StatusDescription.Quotify()) : successMessage.ToString().f(successMsgArgs));

            return new ResponseShim(resp, request, this.BaseUrl, err);
        }



        private bool ParseErr(Exception ex)
        {
            Error_o("Json parser error.");
            Warn_h(ex.Message, "");
            return true;
        }

        private async Task TryUnserialized<T>(IRequestShim req) where T : new()
        {
            Warn_o("Parsing error.");

            var expctd = JsonConvert.SerializeObject(new T(), Formatting.Indented);
            Trace_n("Inspecting unserialized...", "expected format:" + L.f + expctd);

            var resp = await ((IClientShim)this).Send(req);

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
