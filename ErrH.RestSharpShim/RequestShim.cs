using System.Collections.Generic;
using System.Net.Http;
using ErrH.Tools.ErrorConstructors;
using ErrH.Tools.Extensions;
using ErrH.Tools.RestServiceShim;
using RestSharp.Portable;

namespace ErrH.RestSharpShim
{
    public class RequestShim : IRequestShim
    {
        public RestMethod Method { get; set; }
        public string Resource { get; set; }
        public string CsrfToken { get; set; }
        public object Body { get; set; }
        //public FileShim    Attachment  { get; set; }

        public Dictionary<string, string> Cookies { get; private set; }
        public Dictionary<string, object> Parameters { get; private set; }


        public RequestShim(string resource, RestMethod method)
        {
            this.Method = method;
            this.Resource = resource;
            this.Cookies = new Dictionary<string, string>();
            this.Parameters = new Dictionary<string, object>();
        }



        /// <summary>
        /// Converts this instance to an IRestRequest
        /// </summary>
        internal IRestRequest UnShim()
        {
            var req = new RestRequest(this.Resource, Unshim(this.Method));

            if (!this.CsrfToken.IsBlank())
                req.AddHeader("X-CSRF-Token", this.CsrfToken);

            if (this.Body != null)
                req.AddJsonBody(this.Body);

            if (this.Cookies != null)
                foreach (var cookie in this.Cookies)
                    req.AddParameter(cookie.Key, cookie.Value, ParameterType.Cookie);

            if (this.Parameters != null)
            {
                //if (this.Parameters.Count > 0)
                //	req.AddHeader("Content-Type", "application/x-www-form-urlencoded");

                foreach (var param in this.Parameters)
                    req.AddParameter(param.Key, param.Value, ParameterType.RequestBody);
            }



            //later: try this for attaching files to nodes:
            //req.ContentCollectionMode = ContentCollectionMode.MultiPartForFileParameters;


            //req.RequestFormat = DataFormat.Json; <--- Drupal ignores this

            //if (this.Attachment != null)
            //{
            //	req.AddFile(this.Attachment.Name, 
            //				this.Attachment.Stream, 
            //				this.Attachment.Name, 
            //				MediaTypeHeaderValue.Parse("multipart/form-data"));

            //	req.AddParameter("field_name", "field_private_file");//hack: hard-code!
            //	//req.AddParameter("files[files]", "@" + this.Attachment.Name);
            //}

            return req;
        }


        private HttpMethod Unshim(RestMethod restMethod)
        {
            switch (restMethod)
            {
                case RestMethod.Get: return HttpMethod.Get;
                case RestMethod.Post: return HttpMethod.Post;
                case RestMethod.Put: return HttpMethod.Put;
                case RestMethod.Delete: return HttpMethod.Delete;
                default:
                    throw Error.Unsupported(restMethod, "RestMethod");
            }
        }


        public override string ToString()
        {
            var msg = this.GetType().Name + L.f
                + $"[{this.Method.ToString().ToUpper()}] {this.Resource}";

            msg += L.f + "CsrfToken \t:  " + (this.CsrfToken.IsBlank() ? "-blank-" : this.CsrfToken);

            if (this.Body != null)
                msg += L.F + "Body \t:  " + this.Body.ToString();

            if (this.Parameters != null)
            {
                msg += L.F + $"Parameters ({this.Parameters.Count}) :";
                foreach (var p in this.Parameters)
                    msg += L.f + $"{p.Key}  =  {p.Value}";
            }

            return msg;
        }


        public static RequestShim GET(string resource)
        {
            return new RequestShim(resource, RestMethod.Get);
        }

        public static RequestShim POST(string resource)
        {
            return new RequestShim(resource, RestMethod.Post);
        }

        public static RequestShim PUT(string resource)
        {
            return new RequestShim(resource, RestMethod.Put);
        }

        public static RequestShim DELETE(string resource)
        {
            return new RequestShim(resource, RestMethod.Delete);
        }
    }
}
