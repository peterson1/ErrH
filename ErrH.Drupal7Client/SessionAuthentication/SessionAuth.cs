using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ErrH.RestSharpShim;
using ErrH.Tools.Drupal7Models.DTOs;
using ErrH.Tools.Extensions;
using ErrH.Tools.Loggers;
using ErrH.Tools.RestServiceShim;

namespace ErrH.Drupal7Client.SessionAuthentication
{
    internal class SessionAuth : LogSourceBase
    {
        internal D7UserSession Current { get; set; }



        internal async Task<bool> OpenNewSession(IClientShim client, string userName, string password, CancellationToken cancelToken)
        {
            var req = RequestShim.POST(URL.Api_UserLogin);
            req.CsrfToken = await GetToken(client, cancelToken);
            var user = await ValidateCredentials(client, userName, password, req, cancelToken);
            this.Current = await GetUserSession(client, user, cancelToken);
            return this.IsLoggedIn;
        }





        private async Task<D7UserSession> ValidateCredentials(IClientShim client, string u, string p, RequestShim req, CancellationToken cancelToken)
        {
            req.Body = new { username = u, password = Saltify(p) };
            return await client.Send<D7UserSession>(req, cancelToken);
        }


        private string Saltify(string visiblePwd)
        {
            var actualPwd = visiblePwd.ToUpper().Repeat(3).SHA1();
            //Warn(visiblePwd + L.F + actualPwd);
            return actualPwd;
        }


        internal RequestFactory Req 
            => new RequestFactory(this.Current);

        internal bool IsLoggedIn 
            => this.Current != null;



        private async Task<D7UserSession> GetUserSession(IClientShim client, D7UserSession usr, CancellationToken cancelToken)
        {
            var req = RequestFactory.Make(URL.Api_SystemConnect, RestMethod.Post, usr);
            var sess = await client.Send<D7UserSession>(req, cancelToken);
            sess.token = usr.token;
            sess.BaseURL = client.BaseUrl;
            return sess;
        }


        private async Task<string> GetToken(IClientShim client, CancellationToken cancelToken)
        {
            var req = RequestShim.POST(URL.Api_UserToken);
            var tok = await client.Send<D7SessionToken>(req, cancelToken);
            return tok.token;
        }


        internal async Task<bool> CloseSession(IClientShim client, CancellationToken cancelToken)
        {
            Debug_n("Closing user session...", "");
            var req = Req.POST(URL.Api_UserLogout);
            var resp = await client.Send<List<bool>>(req, cancelToken);
            var ok = resp.FirstOrDefault();

            this.Current = null;

            if (ok) return Debug_n("Session successfully closed.", "");
            else return Warn_n("Unexpected logout reply.", ok);
        }








    }
}
