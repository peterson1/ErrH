using System.Collections.Generic;
using System.Threading.Tasks;
using ErrH.Tools.Loggers;

namespace ErrH.Tools.FileSynchronization
{
    public interface IFileSynchronizer : ILogSource
    {
        Task<bool> Run(int folderID, List<RemoteVsLocalFile> filesList, string serverDir);
    }
}
