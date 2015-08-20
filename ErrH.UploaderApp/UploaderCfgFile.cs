using System.Collections.Generic;
using System.Linq;
using ErrH.Configuration;
using ErrH.Tools.ErrorConstructors;
using ErrH.Tools.FileSystemShims;
using ErrH.Tools.Serialization;
using ErrH.UploaderApp.DTOs;
using ErrH.UploaderApp.Models;
using ErrH.UploaderApp.Services;

namespace ErrH.UploaderApp
{
    public class UploaderCfgFile : LocalCfgFile
    {
        public UploaderCfgFile(IFileSystemShim fsShim, 
                               ISerializer serializer)
            : base(fsShim, serializer) { }


        public List<AppFolder> LocalApps
        {
            get
            {
                if (_dto == null) Throw.BadAct(
                    "Call UploaderCfgFile.ReadFrom() before LocalApps().");

                return ((UploaderCfgFileDto)_dto).local_apps
                        .Select(x => {
                            return new AppFolder
                            {
                                Nid = x.app_nid,
                                Alias = x.app_alias,
                                Path = x.local_dir
                            };
                    }).ToList();
            }
        }


        public List<AppFile> FindFiles(AppFolder app)
        {
            var list = new List<AppFile>();
            var files = _fs.Folder(app.Path).Files;
            //if (files == null) return null;

            foreach (var file in AppDirFilter.Declutter(files))
            {
                list.Add(new AppFile(file.Name));
            }
            return list;
        }
    }
}
