using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ErrH.Tools.Drupal7Models;
using ErrH.Tools.Loggers;

namespace ErrH.Tools.FileSynchronization
{
    public interface IFileSynchronizer : ILogSource
    {
        void SetClient(ID7Client d7Client);

        Task<bool> Run( int folderID
                      , List<RemoteVsLocalFile> filesList
                      , string serverDir
                      , CancellationToken cancelToken
                      , string subUrlPattern);

        bool HasReplacement { get; }
    }
}
