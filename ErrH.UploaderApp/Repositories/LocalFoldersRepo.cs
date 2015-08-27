﻿using System;
using System.Collections.Generic;
using ErrH.Configuration;
using ErrH.Tools.CollectionShims;
using ErrH.UploaderApp.DTOs;
using ErrH.UploaderApp.Models;

namespace ErrH.UploaderApp.Repositories
{
    public class LocalFoldersRepo : ListRepoBase<AppFolder>
    {
        private IConfigFile _cfgFile;


        public LocalFoldersRepo(IConfigFile uploaderCfg)
        {
            _cfgFile = uploaderCfg;
        }



        protected override List<AppFolder> LoadList(object[] args)
        {
            var foldr = args?[0]?.ToString() ?? "";

            List<AppFolder> list = null;
            if (!_cfgFile.ReadFrom<UploaderCfgFileDto>
                (foldr)) return list;

            //hack: anti-pattern
            list = ((UploaderCfgFile)_cfgFile).LocalApps;
            return list;
        }


        protected override Func<AppFolder, object> 
            GetKey => x => x.Alias;

    }
}