using System;
using System.Threading.Tasks;
using ErrH.Tools.Drupal7Shim.Entities;
using ErrH.Tools.FileSystemShims;
using ErrH.Tools.Loggers;

namespace ErrH.Tools.Drupal7Shim
{
    public interface ID7Client : ILogSource
    {
        string BaseUrl { get; }

        Task<bool> Login(string baseUrl, string userName, string password);
        bool IsLoggedIn { get; }
        Task<bool> Logout();
        D7User CurrentUser { get; }
        void SaveSession();
        void LoadSession();

        //Task<T>  Get__  <T>(string resource, params object[] args) where T : new();

        Task<T> Get<T>(string resource, string taskTitle = null,
                                        string successMsg = null,
                                        params Func<T, object>[] successMsgArgs
                                        ) where T : new();


        Task<T> Post<T>(T d7Node) where T : D7NodeBase, new();


        Task<T> Put<T>(T nodeRevision, string taskTitle = null, string successMessage = null, params Func<T, object>[] successMsgArgs) where T : ID7NodeRevision, new();

        Task<T> Node<T>(int nodeId) where T : D7NodeBase, new();


        Task<bool> DeleteFile(int fid);


        // Updates specific fields of a node.
        //Task<bool> Put(int nid, Dictionary<string, object> parameters);




        // Attaches file to node.
        //Task<bool>  Post  (FileShim file, int nid);


        /// <summary>
        /// Uploads file to server.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="serverFoldr"></param>
        /// <param name="isPrivate"></param>
        /// <returns>Returns fid on success, otherwise -1.</returns>
        Task<int> Post(FileShim file,
                            string serverFoldr = "",
                            bool isPrivate = true);
    }
}
