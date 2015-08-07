using System;
using System.Net;
using System.Text;
using ErrH.Tools.Extensions;
using ErrH.Tools.RestServiceShim;
using RestSharp.Portable;

namespace ErrH.RestSharpShim
{
    public class ResponseShim : IResponseShim
    {
        public IRequestShim Request { get; private set; }
        public HttpStatusCode Code { get; private set; }
        public string Message { get; private set; }
        public string BaseUrl { get; private set; }
        public string Content { get; private set; }
        public bool IsSuccess { get; private set; }
        public Exception Error { get; private set; }


        internal ResponseShim(IRestResponse resp,
                              IRequestShim req,
                              string baseUrl,
                              Exception error)
        {
            this.Request = req;

            if (resp != null)
            {
                this.Code = resp.StatusCode;
                this.Message = resp.StatusDescription;
                //this.Content = Convert.ToBase64String(resp.RawBytes);
                this.Content = UTF8Encoding.UTF8.GetString(
                                resp.RawBytes, 0, resp.RawBytes.Length);
                this.IsSuccess = resp.IsSuccess;
            }

            this.BaseUrl = baseUrl;
            this.Error = error;

            var restErr = error as RestServiceException;
            if (restErr != null)
            {
                this.Code = restErr.Code;
                this.Message = restErr.Message;
            }
        }



        public override string ToString()
        {
            //var s = MsgLine("response",
            //		"[{0}: {1}]  {2}", (int)this.Code,
            //							this.Code,
            //							this.Message);
            var s = $"[{(int)this.Code}: {this.Code}]".AlignRight(25)
                  + "   " + this.Message + L.F;

            if (this.Request != null)
                s += MsgLine("resource", "[{0}]  {1}",
                              Request.Method.ToString().ToUpper(),
                              Request.Resource);

            s += MsgLine("base URL", this.BaseUrl);

            if (this.Request != null)
            {
                s += MsgLine("body", (Request.Body == null)
                        ? "-null-" : Request.Body.ToString());

                s += MsgLine("x-csrf token", Request.CsrfToken.IsBlank()
                        ? "-blank-" : Request.CsrfToken);
            }

            if (this.Error != null)
                s += L.f + this.Error.Message(false, false);

            return s;
        }

        private string MsgLine(string label, object value, params object[] args)
        {
            var lbl = (label + "  :  ").AlignRight(29);
            var val = value.ToString().f(args);
            return lbl + val + L.f;
        }

    }
}
