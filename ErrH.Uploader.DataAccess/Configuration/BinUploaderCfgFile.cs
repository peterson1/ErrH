using System.Collections.Generic;
using ErrH.Tools.Authentication;
using ErrH.Tools.FileSynchronization;
using ErrH.Tools.FileSystemShims;
using ErrH.Tools.Serialization;

namespace ErrH.Uploader.DataAccess.Configuration
{
    public class BinUploaderCfgFile : LoginCfgFile
    {
        public List<SyncableFolderInfo> LocalApps { get; set; }



        public BinUploaderCfgFile(IFileSystemShim fileSystemShim, ISerializer serializer) 
            : base(fileSystemShim, serializer) { }


        public override bool ReadFrom(string fileName)
        {
            var typd = ReadAs<BinUploaderCfgFile>(fileName);
            if (typd == null) return false;

            LocalApps = typd.LocalApps;
            return true;
        }
    }
}
