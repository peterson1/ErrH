using ErrH.RestSharpShim;
using ErrH.Tools.Drupal7Models.DTOs;
using ErrH.Tools.Extensions;
using ErrH.Tools.RestServiceShim;

namespace ErrH.Drupal7Client.SessionAuthentication
{
    internal class RequestFactory
    {
        private D7UserSession _sess;
        private string _userName;
        private string _password;

        public RequestFactory(D7UserSession userSession, string usr, string pwd)
        {
            this._sess = userSession;
            this._userName = usr;
            this._password = pwd;
        }

        internal IRequestShim GET(string resource, params object[] args)
            => Make(resource.f(args), RestMethod.Get, _sess, _userName, _password);

        internal IRequestShim POST(string resource, params object[] args)
            => Make(resource.f(args), RestMethod.Post, _sess, _userName, _password);

        internal IRequestShim PUT(string resource, params object[] args)
            => Make(resource.f(args), RestMethod.Put, _sess, _userName, _password);

        internal IRequestShim DELETE(string resource, params object[] args)
            => Make(resource.f(args), RestMethod.Delete, _sess, _userName, _password);


        internal static IRequestShim Make(string resource, RestMethod method, D7UserSession sess, string usr, string pwd)
        {
            var req = new RequestShim(resource, method);
            req.UserName = usr;
            req.Password = pwd;
            req.CsrfToken = sess.token;
            req.Cookies.Add(sess.session_name, sess.sessid);
            return req;
        }
    }
}
