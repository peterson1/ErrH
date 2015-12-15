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
        internal async Task<bool> Post<T>(T[] d7Nodes, CancellationToken tkn, IClientShim client, SessionAuth auth) where T : ID7Node, new()
        {
            if (!auth.IsLoggedIn) return Error_n("D7 client is not logged in.", "");

            var list = new List<IRequestShim>();
            foreach (var nod in d7Nodes)
            {
                var req  = auth.Req.POST(URL.Api_EntityNode);
                nod.uid  = auth.Current.user.uid;
                req.Body = nod;
                list.Add(req);
            }
            return await client.Send<T>(tkn, list);
        }
    }
}
