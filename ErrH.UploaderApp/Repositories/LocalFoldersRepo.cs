using System;
using System.Collections.Generic;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.ScalarEventArgs;
using ErrH.UploaderApp.DTOs;
using ErrH.UploaderApp.Models;

namespace ErrH.UploaderApp.Repositories
{
    public class LocalFoldersRepo : ListRepoBase<AppFolder>, IFoldersRepo
    {
        public event EventHandler<UrlEventArg> CertSelfSigned;

        private UploaderCfgFile _cfgFile;


        public LocalFoldersRepo(UploaderCfgFile uploaderCfg)
        {
            _cfgFile = ForwardLogs(uploaderCfg);

            _cfgFile.CertSelfSigned += (s, e)
                => { CertSelfSigned?.Invoke(s, e); };
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
