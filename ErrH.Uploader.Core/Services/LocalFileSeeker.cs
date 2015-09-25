using System.Collections.Generic;
using ErrH.Tools.FileSynchronization;
using ErrH.Tools.FileSystemShims;
using ErrH.Tools.Loggers;

namespace ErrH.Uploader.Core.Services
{
    public class LocalFileSeeker : LogSourceBase
    {
        private IFileSystemShim _fsShim;

        public LocalFileSeeker(IFileSystemShim fsShim)
        {
            _fsShim = fsShim;
        }


        public List<SyncableFileLocal> GetFiles
            (string folderPath, string pattern = "*.*")
        {
            var list  = new List<SyncableFileLocal>();
            List<FileShim> files; string msg;

            if (!_fsShim.TryGetDirFiles
                (folderPath, pattern, out files, out msg))
                    return Warn_(list, "Unable to get list of files.", msg);

            foreach (var file in files.Declutter())
            {
                list.Add(new SyncableFileLocal
                {
                    Name    = file.Name,
                    Size    = file.Size,
                    Version = file.Version,
                    SHA1    = file.SHA1,
                    Path    = file.Path
                });
            }
            return list;
        }
    }
}
