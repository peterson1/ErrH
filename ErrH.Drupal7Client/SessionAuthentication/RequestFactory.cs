using ErrH.RestSharpShim;
using ErrH.Tools.Drupal7Models.DTOs;
using ErrH.Tools.Extensions;
using ErrH.Tools.RestServiceShim;

namespace ErrH.Drupal7Client.SessionAuthentication
{
    internal class RequestFactory
    {
        private D7UserSession _sess;


        internal RequestFactory(D7UserSession userSession)
        {
            this._sess = userSession;
        }


        internal IRequestShim GET(string resource, params object[] args)
        {
            return Make(resource.f(args), RestMethod.Get, _sess);
        }

        internal IRequestShim POST(string resource, params object[] args)
        {
            return Make(resource.f(args), RestMethod.Post, _sess);
        }

        internal IRequestShim PUT(string resource, params object[] args)
        {
            return Make(resource.f(args), RestMethod.Put, _sess);
        }

        internal IRequestShim DELETE(string resource, params object[] args)
        {
            return Make(resource.f(args), RestMethod.Delete, _sess);
        }


        internal static IRequestShim Make(string resource, RestMethod method, D7UserSession sess)
        {
            var req = new RequestShim(resource, method);
            req.CsrfToken = sess.token;
            req.Cookies.Add(sess.session_name, sess.sessid);
            return req;
        }
    }
}
