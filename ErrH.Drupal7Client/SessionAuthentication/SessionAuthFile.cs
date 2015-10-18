using ErrH.Tools.Drupal7Models.DTOs;
using ErrH.Tools.Extensions;
using ErrH.Tools.FileSystemShims;
using ErrH.Tools.Serialization;

namespace ErrH.Drupal7Client.SessionAuthentication
{/*
    internal class SessionAuthFile
    {
        const string _sessionFile = ".session";
        const string _sessionDir = "Shim.D7RestPCL4.Modules.Services.SessionAuth";
        const SpecialDir _specialDir = SpecialDir.LocalApplicationData;


        internal static D7UserSession Read(IFileSystemShim fsShim, ISerializer serializr)
        {
            var file = SessionFile(fsShim);
            if (!file.Found) return null;
            return serializr.Read<D7UserSession>(file);
        }


        internal static bool Write(D7UserSession session, IFileSystemShim fsShim, ISerializer serializr)
        {
            var content = serializr.Write(session, true);
            var file = SessionFile(fsShim);
            if (!file.Write(content, EncodeAs.UTF8)) return false;
            file.Hidden = true;
            return true;
        }


        internal static bool Found(IFileSystemShim fsShim)
            => fsShim.IsFileFound(SessionFile(fsShim).Path);



        internal static bool Delete(IFileSystemShim fsShim)
            => SessionFile(fsShim).Delete();


        private static FileShim SessionFile(IFileSystemShim fsShim)
        {
            var dirPath = fsShim.GetSpecialDir(_specialDir).Bslash(_sessionDir);
            return fsShim.Folder(dirPath).File(_sessionFile, false);
        }
    }*/
}
