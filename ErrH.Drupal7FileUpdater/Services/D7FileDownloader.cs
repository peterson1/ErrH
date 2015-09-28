using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ErrH.Drupal7FileUpdater.DTOs;
using ErrH.Tools.Drupal7Models;
using ErrH.Tools.Extensions;
using ErrH.Tools.FileSynchronization;
using ErrH.Tools.FileSystemShims;
using ErrH.Tools.Loggers;

namespace ErrH.Drupal7FileUpdater.Services
{
    internal class D7FileDownloader : LogSourceBase
    {
        private ID7Client  _client;
        private FolderShim _foldr;
        private string     _urlPattern;


        internal D7FileDownloader(string targetDir, string subUrlPattern, IFileSystemShim fsShim, ID7Client d7Client)
        {
            _client     = ForwardLogs(d7Client);
            _foldr      = fsShim.Folder(targetDir);
            _urlPattern = subUrlPattern;
        }


        private string ContentUrl(int fid) => _urlPattern.f(fid);


        internal async Task<bool> CreateFile(RemoteVsLocalFile inf, CancellationToken cancelToken)
        {
            var locF = _foldr.File(inf.Filename, false);
            if (locF.Found) return Error_n("File already exists in target folder.", "Use ReplaceFile() method instead.");

            var rem = inf.Remote;
            var dto = await _client.Get<List<FileContentDto>>(ContentUrl(rem.Fid), cancelToken);
            if (dto == null) return Error_n("_client.Get<List<FileContentDto>>() == NULL", "");
            if (dto.Count == 0) return Warn_n("Remote file not found in server.", $"[nid: {rem.Nid}]  {rem.Name}");
            if (dto.Count > 1) return Warn_n("Duplicate files found in server.", string.Join(", ", dto.Select(x => x.fid)));

            if (!locF.Write(dto[0].b64.Base64ToBytes())) return false;

            //todo: verify downloaded file

            return true;
        }


    }
}
