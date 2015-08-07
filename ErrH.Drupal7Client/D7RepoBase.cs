using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrH.Tools.Drupal7Models;
using ErrH.Tools.Extensions;
using ErrH.Tools.Loggers;

namespace ErrH.Drupal7Client
{
    public class D7RepoBase<T> : LogSourceBase, ID7Repo
    {
        private string _resource;
        private ReadOnlyCollection<T> _data;

        protected ID7Client _client;


        public ReadOnlyCollection<T> Data
        {
            get
            {
                if (_data == null)
                    Error_n("Data is currently empty.",
     "Call {0}() before using Data.", this.IsConnected
                         ? "RefreshData" : "Connect");
                return _data;
            }
        }


        public D7RepoBase(ID7Client d7Client)
        {
            _client = ForwardLogs(d7Client);
        }



        /// <summary>
        /// Returns true rightaway if already connected -- so it's safe to call this repeatedly on your code.
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        public async Task<bool> Connect(string baseUrl, string username,
                                        string password, string resource)
        {
            if (!(await this._client.Login(
                baseUrl, username, password)))
                return false;

            this._resource = resource;
            //if (!(await this.RefreshData(queryParams))) return false;

            return this.IsConnected;
        }




        public virtual async Task<bool> RefreshData(params object[] queryParams)
        {
            Debug_n("Refreshing repository data from source...", "");

            if (!this.IsConnected)
                return Error_n("Currently disconnected from data source.", "Call Connect() before RefreshData().");

            var rsrc = (queryParams == null) ? _resource
                     : _resource.Slash(string.Join("/", queryParams));

            var list = await _client.Get<List<T>>(rsrc, null,
                "Successfully refreshed repository data ({0} fetched).",
                x => "{0:record}".f(x.Count));

            if (list == null) return false;

            _data = new ReadOnlyCollection<T>(list);
            return true;
        }



        public bool IsConnected
        {
            get
            {
                if (_client == null) return false;
                return _client.IsLoggedIn;
            }
        }



        public async Task<bool> Disconnect()
        {
            if (_client == null) return false;
            return await _client.Logout();
        }




        public void Dispose()
        {
            _data = null;
            //await this.Disconnect();
        }

    }
}
