using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrH.Tools.Drupal7Models;
using ErrH.Tools.Extensions;
using ErrH.Tools.FileSystemShims;
using ErrH.Tools.Loggers;
using ErrH.Uploader.Core;
using ErrH.Uploader.Core.Nodes;

namespace ErrH.Uploader.DataAccess.AppFileRepository
{
    public class AppFileRepo_Downloader : LogSourceBase
    {
        private ID7Client _client;
        private IFileSystemShim _fs;


        public AppFileRepo_Downloader(ID7Client d7Client, IFileSystemShim fsShim)
        {
            this._client = d7Client;
            this._fs = fsShim;
        }


        internal async Task<FileShim> Download(AppFileNode node, FolderShim foldr)
        {
            var dto = await _client.Get<List<AppFileRepo_DownloaderDto>>(
                                        URL.file_content_x.f(node.Fid));
            if (dto == null) return Error_<FileShim>(null, "Remote request returned NULL.", "Expected a list of file DTOs.");
            if (dto.Count == 0) return Error_<FileShim>(null, "Remote file not found in server.", "[nid: {0}]  {1}", node.Nid, node.Name);
            if (dto.Count > 1) return Error_<FileShim>(null, "Duplicate files found in server.", string.Join(", ", dto.Select(x => x.fid)));

            var locF = foldr.File(node.Name, false);
            if (!locF.Write(dto[0].b64.Base64ToBytes())) return null;

            if (!VerifyDownloaded(locF, node)) return locF = null;

            return locF;
        }

        //todo: reuse logic in AppFileItemExtensions
        private bool VerifyDownloaded(FileShim actual, AppFileNode expctd)
        {
            var s = "Downloaded files is corrupted.";

            if (actual.Name != expctd.Name)
                return Error_n(s,
"expected name: {0}  ==>  actual: {1}", expctd.Name, actual.Name);

            if (actual.Size != expctd.Size)
                return Error_n(s,
"expected size: {0}  ==>  actual: {1}", expctd.Size.KB(), actual.Size.KB());

            if (actual.SHA1 != expctd.SHA1)
                return Error_n(s,
"expected hash: {0}  ==>  actual: {1}", expctd.SHA1, actual.SHA1);

            return true;
        }

    }



    public class AppFileRepo_DownloaderDto
    {
        public int fid { get; set; }

        /// <summary>
        /// Base64-encoded content
        /// </summary>
        public string b64 { get; set; }
    }
}
