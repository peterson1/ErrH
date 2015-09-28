using System.Collections.Generic;
using System.Linq;
using ErrH.BinUpdater.Core.DTOs;
using ErrH.Tools.ErrorConstructors;
using ErrH.Tools.FileSynchronization;
using ErrH.Tools.FileSystemShims;
using ErrH.Tools.Serialization;

namespace ErrH.BinUpdater.Core.Configuration
{
    public class UploaderCfgFile : LocalCfgFile
    {
        public UploaderCfgFile(IFileSystemShim fsShim,
                               ISerializer serializer)
            : base(fsShim, serializer)
        { }


        public List<SyncableFolderInfo> LocalApps
        {
            get
            {
                if (_dto == null)
                    Throw.BadAct("Call UploaderCfgFile.ReadFrom() before LocalApps().");

                return ((UploaderCfgFileDto)_dto).local_apps
                        .Select(x => {
                            return new SyncableFolderInfo
                            {
                                Nid = x.app_nid,
                                Alias = x.app_alias,
                                Path = x.local_dir
                            };
                        }).ToList();
            }
        }


        //public List<AppFileDiff> FindFiles(AppFolder app)
        //{
        //    var list = new List<AppFileDiff>();
        //    var files = _fs.Folder(app.Path).Files;
        //    //if (files == null) return null;

        //    foreach (var file in AppDirFilter.Declutter(files))
        //    {
        //        list.Add(new AppFileDiff(file.Name));
        //    }
        //    return list;
        //}
    }
}
