using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ErrH.Drupal7Client.SessionAuthentication;
using ErrH.Tools.Drupal7Models.Entities;
using ErrH.Tools.Loggers;
using ErrH.Tools.RestServiceShim;

namespace ErrH.Drupal7Client.BatchOperations
{
    internal class BatchSender : LogSourceBase
    {
        private SessionAuth _auth;
        private IClientShim _client;

        public BatchSender(IClientShim clientShim, SessionAuth sessionAuth)
        {
            _client = clientShim;
            _auth   = sessionAuth;
        }

        internal async Task<bool> Post<T>( IEnumerable<T> d7Nodes
                                         , int pageSize
                                         , CancellationToken tkn
        ) where T : ID7Node, new()
        {
            if (!_auth.IsLoggedIn) return Error_n("D7 client is not logged in.", "");

            var list = new List<IRequestShim>();
            foreach (var nod in d7Nodes)
            {
                var req  = _auth.Req.POST(URL.Api_EntityNode);
                nod.uid  = _auth.Current.user.uid;
                req.Body = nod;
                list.Add(req);
            }
            return await _client.Send<T>(list, pageSize, tkn);
        }
    }
}
