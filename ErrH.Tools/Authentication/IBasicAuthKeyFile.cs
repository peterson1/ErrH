using ErrH.Tools.FileSystemShims;
using ErrH.Tools.Loggers;

namespace ErrH.Tools.Authentication
{
    public interface IBasicAuthKeyFile : IBasicAuthenticationKey, ILogSource
    {
        IBasicAuthenticationKey ReadFrom (FolderShim folder);

        bool CreateIn(FolderShim folder, 
                      string baseUrl, 
                      string userName);

        FileShim  File  { get; }

        string TempPassword { set; }
    }
}
