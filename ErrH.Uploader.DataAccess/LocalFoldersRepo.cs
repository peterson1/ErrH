using System;
using System.Collections.Generic;
using ErrH.BinUpdater.Core.Configuration;
using ErrH.BinUpdater.Core.DTOs;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.Drupal7Models;
using ErrH.Tools.Extensions;
using ErrH.Tools.FileSynchronization;

namespace ErrH.Uploader.DataAccess
{
    public class LocalFoldersRepo : ListRepoBase<SyncableFolderInfo>
    {
        private UploaderCfgFile _cfgFile;


        public LocalFoldersRepo(IConfigFile uploaderCfg, ID7Client d7Client)
        {
            _cfgFile = uploaderCfg.As<UploaderCfgFile>();
            ForwardLogs(_cfgFile);
            ForwardLogs(d7Client);

            //_cfgFile.CredentialsReady
            //    += d7Client.LoginUsingCredentials;
        }



        protected override List<SyncableFolderInfo> LoadList(object[] args)
        {
            var foldr = args?[0]?.ToString() ?? "";

            List<SyncableFolderInfo> list = null;
            if (!_cfgFile.ReadFrom<UploaderCfgFileDto>(foldr))
                return list;

            list = _cfgFile.LocalApps;
            return list;
        }



        protected override Func<SyncableFolderInfo, object>
            GetKey => x => x.Alias;

    }
}
