using System.Collections.Generic;
using ErrH.Tools.FileSystemShims;
using ErrH.Uploader.Core.Models;

namespace ErrH.Uploader.Core.Services
{
    public class LocalFileSeeker
    {
        private IFileSystemShim _fsShim;

        public LocalFileSeeker(IFileSystemShim fsShim)
        {
            _fsShim = fsShim;
        }


        public List<AppFileInfo> GetFiles(string folderPath)
        {
            var list  = new List<AppFileInfo>();
            var files = _fsShim.Folder(folderPath).Files;

            foreach (var file in files.Declutter())
            {
                list.Add(new AppFileInfo
                {
                    Name    = file.Name,
                    Size    = file.Size,
                    Version = file.Version,
                    SHA1    = file.SHA1
                });
            }
            return list;
        }
    }
}
