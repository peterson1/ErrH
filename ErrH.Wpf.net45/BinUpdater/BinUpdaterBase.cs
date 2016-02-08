using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ErrH.Tools.Extensions;
using ErrH.Wpf.net45.Extensions;
using PropertyChanged;

namespace ErrH.Wpf.net45.BinUpdater
{
    [ImplementPropertyChanged]
    public abstract class BinUpdaterBase
    {
        private const string REPLACED_DIR = "Replaced";

        public event EventHandler DisplacingOutdateds;
        public event EventHandler DownloadingUpdates;
        public event EventHandler UpdatesInstalled;

        public string Status    { get; set; }         = "Idle.";
        public bool   IsEnabled { get; private set; } = false;

        protected abstract Task<T>  ApiGet<T>  (string url, BinUpdaterKey cfg);
        protected abstract bool     LogError   (string v);

        public async void StartChecking(BinUpdaterKey cfg)
        {
            ClearReplacementsDir();

            IsEnabled = true;
            while (IsEnabled)
            {
                var changes = await GetChangedFiles(cfg);
                Status = $"Checked for newer version. {changes.Count.x("updatable file")} found.";
                if (changes.Count > 0)
                {
                    IsEnabled = false;
                    DisplacingOutdateds?.Invoke(this, EventArgs.Empty);
                    Status = "Updates found. Displacing outdated files...";

                    DisplaceOutdatedFiles(changes);
                    DownloadingUpdates?.Invoke(this, EventArgs.Empty);
                    Status = "Outdateds displaced. Downloading updates...";

                    await DownloadUpdates(changes, cfg);
                    UpdatesInstalled?.Invoke(this, EventArgs.Empty);
                    Status = "Updates downloaded and installed.";
                    break;
                }
                await DelayRetry(cfg.EveryMin);
            }
        }


        private void ClearReplacementsDir()
        {
            Status = "Clearing temporary folder...";
            var dir = AppDomain.CurrentDomain.BaseDirectory;
            dir = Path.Combine(dir, REPLACED_DIR);
            if (Directory.Exists(dir))
            {
                foreach (var file in Directory.GetFiles(dir))
                    File.Delete(file);
            }
            else
                Directory.CreateDirectory(dir);
        }


        private async Task<List<AppFilesDTO>> GetChangedFiles(BinUpdaterKey cfg)
        {
            Status = $"Checking for updates as “{cfg.Username}”...";
            var rsrc = AppFilesURL(cfg.AppNid);
            var list = await ApiGet<List<AppFilesDTO>>(rsrc, cfg);

            foreach (var f in list)
                f.FileDiffered += (s, e) => Status = e.Value;

            list?.RemoveAll(x => x.IsSame());
            return list;
        }


        protected virtual string AppFilesURL(int appNid)
            => $"views/repo_data_source?display_id=page_1&args={appNid}";


        protected virtual string FileContentURL(int fileFid)
            => $"views/app_file_repo?display_id=page_1&args={fileFid}";



        private void DisplaceOutdatedFiles(List<AppFilesDTO> changedFiles)
        {
            foreach (var appFile in changedFiles)
            {
                var file   = appFile.FindLocalFile();
                if (!file.Exists) continue;
                var newLoc = DisplacementPath(file);
                file.MoveTo(newLoc);
            }
        }


        private string DisplacementPath(FileInfo file)
        {
            var bseDir = file.Directory.FullName;
            var repDir = Path.Combine(bseDir, REPLACED_DIR);
            var suffix = DateTime.Now.TimeOfDay.TotalMinutes;
            var newNme = file.Name + "_" + suffix;
            return Path.Combine(repDir, newNme);
        }


        private async Task DownloadUpdates(List<AppFilesDTO> changedFiles, BinUpdaterKey cfg)
        {
            foreach (var file in changedFiles)
            {
                if (!file.app_file_fid.HasValue) continue;

                var rsrc   = FileContentURL(file.app_file_fid.Value);
                FetchFile:
                var contnt = await ApiGet<List<FileContentDTO>>(rsrc, cfg);
                var byts   = DecodeDownloaded(contnt);

                if (byts != null)
                    File.WriteAllBytes(file.LocalPath, byts);

                if (!VerifyWritten(file)) goto FetchFile;
            }
        }


        private byte[] DecodeDownloaded(List<FileContentDTO> contnt)
        {
            byte[] ret = null;
            if (contnt == null)
            {
                LogError($"Downloaded content array is NULL.");
                return ret;
            }
            if (contnt?.Count != 1)
            {
                LogError($"Expected downloaded content array to have 1 item, but found {contnt.Count}.");
                return ret;
            }
            return contnt[0].b64.Base64ToBytes();
        }


        private bool VerifyWritten(AppFilesDTO file)
        {
            var actual = file.FindLocalFile();
            var path   = L.f + actual.FullName;

            if (!actual.Exists)
                return LogError($"Downloaded file not written." + path);

            if (actual.Length != file.app_file_size)
                return LogError($"Expected written size to be {file.app_file_size} but was {actual.Length}." + path);

            if (actual.SHA1() != file.app_file_sha1)
                return LogError($"Hash mismatch in written file." + path);

            return true;
        }


        private async Task DelayRetry(int minutes)
        {
            for (int i = minutes; i > 0; i--)
            {
                Status = $"Next update check in {i.x("min")}. ...";
                await Task.Delay(1000 * 60);
            }
        }
    }
}
