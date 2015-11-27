using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ErrH.Tools.Authentication;
using ErrH.Tools.Drupal7Models.Entities;
using ErrH.Tools.FileSystemShims;
using ErrH.Tools.ScalarEventArgs;

namespace ErrH.Tools.Drupal7Models
{
    public interface ID7Client : ISessionClient
    {
        event EventHandler<EArg<bool>>  ResponseReceived;

        D7User CurrentUser  { get; }

        //void LoginUsingCredentials(object sender, EArg<LoginCredentials> evtArg);



        Task<T> Get<T>(string resource,
                       CancellationToken cancelToken,
                       string taskTitle = null,
                       string successMsg = null,
                       params Func<T, object>[] successMsgArgs
                       ) where T : new();



        Task<T> Post<T>(T d7Node, 
                        CancellationToken cancelToken) 
            where T : D7NodeBase, new();



        Task<T> Put<T>(T nodeRevision,
                       CancellationToken cancelToken,
                       string taskTitle = null, 
                       string successMessage = null, 
                       params Func<T, object>[] successMsgArgs) 
            where T : ID7NodeRevision;


        Task<bool> Put (ID7NodeRevision nodeRevision,
                        CancellationToken token);



        Task<T> Node<T>(int nodeId, 
                        CancellationToken cancelToken) 
            where T : D7NodeBase, new();



        Task<bool> Delete(int nid, 
                          CancellationToken cancelToken);
        
        //Task<bool>  DeleteFile (int fid);


        // Updates specific fields of a node.
        //Task<bool> Put(int nid, Dictionary<string, object> parameters);




        // Attaches file to node.
        //Task<bool>  Post  (FileShim file, int nid);


        /// <summary>
        /// Uploads file to server.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="cancelToken"></param>
        /// <param name="serverFoldr"></param>
        /// <param name="isPrivate"></param>
        /// <returns>Returns fid on success, otherwise -1.</returns>
        Task<int> Post(FileShim file,
                       CancellationToken cancelToken,
                       string serverFoldr = "",
                       bool isPrivate = true);


        IEnumerable<D7Term>  Terms          { get; }
        bool                 IsTermsLoaded  { get; }
        Task<bool>           LoadTerms      (CancellationToken token = new CancellationToken());
    }
}
