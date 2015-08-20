using System;
using System.Collections.Generic;
using ErrH.Tools.CollectionShims;
using ErrH.UploaderApp.DTOs;
using ErrH.UploaderApp.Models;

namespace ErrH.UploaderApp.Repositories
{
    public class LocalFoldersRepo : ListRepoBase<AppFolder>
    {
        private UploaderCfgFile _cfgFile;


        public LocalFoldersRepo(UploaderCfgFile uploaderCfg)
        {
            _cfgFile = ForwardLogs(uploaderCfg);
        }



        protected override bool LoadList(string dataSourceUri, ref List<AppFolder> list)
        {
            if (!_cfgFile.ReadFrom<UploaderCfgFileDto>
                (dataSourceUri)) return false;

            list = _cfgFile.LocalApps;
            return true;
        }


        protected override Func<AppFolder, object> 
            GetKey => x => x.Alias;

    }
}
