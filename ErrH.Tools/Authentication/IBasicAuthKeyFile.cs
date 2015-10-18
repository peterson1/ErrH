using ErrH.Tools.FileSystemShims;

namespace ErrH.Tools.Authentication
{
    public interface IBasicAuthKeyFile : IBasicAuthenticationKey
    {
        IBasicAuthenticationKey ReadFrom (FolderShim folder);

        bool SaveTo(FolderShim folder, 
                    string baseUrl, 
                    string userName);

        FileShim  File  { get; }
    }
}
