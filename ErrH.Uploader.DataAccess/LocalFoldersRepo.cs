using System;
using System.Collections.Generic;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.Converters;
using ErrH.Uploader.Core.Configuration;
using ErrH.Uploader.Core.DTOs;
using ErrH.Uploader.Core.Models;

namespace ErrH.Uploader.DataAccess
{
    public class LocalFoldersRepo : ListRepoBase<AppFolder>
    {
        private UploaderCfgFile _cfgFile;


        public LocalFoldersRepo(IConfigFile uploaderCfg)
        {
            _cfgFile = Cast.As<UploaderCfgFile>(uploaderCfg);
        }



        protected override List<AppFolder> LoadList(object[] args)
        {
            var foldr = args?[0]?.ToString() ?? "";

            List<AppFolder> list = null;
            if (!_cfgFile.ReadFrom<UploaderCfgFileDto>(foldr))
                return list;

            list = _cfgFile.LocalApps;
            return list;
        }



        protected override Func<AppFolder, object>
            GetKey => x => x.Alias;

    }
}
