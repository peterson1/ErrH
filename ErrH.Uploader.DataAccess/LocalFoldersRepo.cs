using System;
using System.Collections.Generic;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.Extensions;
using ErrH.Tools.FileSynchronization;
using ErrH.Uploader.DataAccess.Configuration;

namespace ErrH.Uploader.DataAccess
{
    public class LocalFoldersRepo : ListRepoBase<SyncableFolderInfo>
    {
        private BinUploaderCfgFile _cfgFile;


        public LocalFoldersRepo(BinUploaderCfgFile uploaderCfg)
        {
            _cfgFile = ForwardLogs(uploaderCfg);
        }



        protected override List<SyncableFolderInfo> LoadList(object[] args)
        {
            List<SyncableFolderInfo> list = null;

            var fName = args[0].ToString();
            if (fName.IsBlank())
                return Error_(list, "args[0] should not be blank.",
                                   $"‹{GetType().Name}›LoadList()");

            if (!_cfgFile.ReadFrom(fName)) return null;
            return _cfgFile.LocalApps;
        }



        protected override Func<SyncableFolderInfo, object>
            GetKey => x => x.Alias;

    }
}
