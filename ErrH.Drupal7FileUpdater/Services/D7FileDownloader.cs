using System;
using System.Collections.Generic;
using System.Linq;
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
        private ID7Client        _client;
        private IFileSystemShim  _fs;
        private FolderShim       _foldr;
        private string           _urlPattern;
        private string           _suffix;


        internal D7FileDownloader(string targetDir, string subUrlPattern, IFileSystemShim fsShim, ID7Client d7Client)
        {
            _client     = ForwardLogs(d7Client);
            _fs         = ForwardLogs(fsShim);
            _foldr      = _fs.Folder(targetDir);
            _urlPattern = subUrlPattern;
            _suffix     = "_" + DateTime.Now.TimeOfDay.TotalMinutes;
            DeleteOldReplacements();
        }



        private void DeleteOldReplacements()
        {
            if (!TempDir.Found) return;
            try
            {
                TempDir.Files().ForEach(x => x.Delete());
            }
            catch (Exception ex)
            {
                Error_n("Unable to delete previously replaced files.", ex.Details(false, false));
            }
        }


        private FolderShim TempDir => _foldr.SubFolder("Replaced");


        internal async Task<bool> CreateFile(RemoteVsLocalFile inf, CancellationToken cancelToken)
        {
            var locF = _foldr.File(inf.Filename, false);
            if (locF.Found) return Error_n("File already exists in target folder.", "Use ReplaceFile() method instead.");

            var b64Dto = await DownloadFile(inf.Remote, cancelToken);
            if (b64Dto == null) return false;

            if (!locF.Write(b64Dto.b64.Base64ToBytes())) return false;

            return VerifyFile(locF, inf.Remote);
        }


        internal async Task<bool> ReplaceFile(RemoteVsLocalFile inf, CancellationToken cancelToken)
        {
            var locF = _foldr.File(inf.Filename, false);
            if (!locF.Found) return Error_n("File missing from target folder.", "Use CreateFile() method instead.");

            if (!TempDir.Create())
                return Error_n("Unable to create temporary folder.", TempDir.Path);

            if (!RelocateActiveFile(locF)) return false;

            var b64Dto = await DownloadFile(inf.Remote, cancelToken);
            if (b64Dto == null) return false;

            if (!locF.Write(b64Dto.b64.Base64ToBytes())) return false;

            return VerifyFile(locF, inf.Remote);
        }


        private bool RelocateActiveFile(FileShim locF)
        {
            var newF = TempDir.Path.Bslash(locF.Name) + _suffix;

            if (!locF.MoveTo(newF))
                return Error_n("Unable to move currently in-use file.", locF.Path);
            else
                return true;
        }


        private bool VerifyFile(FileShim actual, SyncableFileBase expctd)
        {
            var s = "Downloaded files is corrupted.";

            if (actual.Name != expctd.Name)
                return Error_n(s, $"expected name: {expctd.Name}  ==>  actual: {actual.Name}");

            if (actual.Size != expctd.Size)
                return Error_n(s, $"expected size: {expctd.Size.KB()}  ==>  actual: {actual.Size.KB()}");

            if (actual.SHA1 != expctd.SHA1)
                return Error_n(s, $"expected hash: {expctd.SHA1}  ==>  actual: {actual.SHA1}");

            return true;
        }


        private async Task<FileContentDto> DownloadFile(SyncableFileRemote node, CancellationToken cancelToken)
        {
            FileContentDto ret = null;

            var list = await _client.Get<List<FileContentDto>>
                        (_urlPattern.f(node.Fid), cancelToken);

            if (list == null)
                return Error_(ret, "_client.Get<List<FileContentDto>>() == NULL", "");

            if (list.Count == 0)
                return Warn_(ret, "Remote file not found in server.", $"[nid: {node.Nid}]  {node.Name}");

            if (list.Count > 1)
                return Warn_(ret, "Duplicate files found in server.", string.Join(", ", list.Select(x => x.fid)));

            return Trace_(list[0], "Successfully downloaded file content from server", $"{node.Name}");
        }
    }
}
