﻿using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using ErrH.BinUpdater.Core;
using ErrH.BinUpdater.Core.Configuration;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.Drupal7Models;
using ErrH.Tools.FileSynchronization;
using ErrH.Tools.ScalarEventArgs;
using ErrH.WinTools.ReflectionTools;
using ErrH.WpfTools.Commands;
using ErrH.WpfTools.ViewModels;
using PropertyChanged;

namespace ErrH.BinUpdater.ViewModels
{
    [ImplementPropertyChanged]
    public class BinUpdaterVM : WorkspaceVmBase
    {
        public event EventHandler<EArg<string>> RestartRequested;


        //private string                          _fileName;
        private BinUpdaterCfgFile               _cfgFile;
        private AppFileGrouper                  _grouper;
        private IFileSynchronizer               _synchronizer;
        private ID7Client                       _d7Client;
        private IRepository<SyncableFileRemote> _remotes;

        public IAsyncCommand      UpdateNowCmd { get; }
        public UpdaterSessionVM   UserSession  { get; }
        public LogScrollerVM      LogScroller  { get; }



        public BinUpdaterVM(IRepository<SyncableFileRemote> filesRepo, 
                            AppFileGrouper fileGrouper, 
                            IFileSynchronizer fileSynchronizer,
                            BinUpdaterCfgFile cfgFile,
                            UpdaterSessionVM usrSessionVm,
                            ID7Client d7Client,
                            LogScrollerVM logScroller)
        {
            DisplayName   = "Bin Updater";
            _d7Client     = ForwardLogs(d7Client);
            _grouper      = ForwardLogs(fileGrouper);
            _synchronizer = ForwardLogs(fileSynchronizer);
            _remotes      = ForwardLogs(filesRepo);
            _cfgFile      = ForwardLogs(cfgFile);
            UserSession   = ForwardLogs(usrSessionVm);
            UpdateNowCmd  = AsyncCommand.Create(tkn => UpdateNow(tkn));

            LogScroller = logScroller.ListenTo(this);
        }


        //public bool ReadFrom(string cfgFileName)
        //{
        //    if (!UserSession.AuthFile.ReadFrom(cfgFileName)) return false;
        //    return true;
        //}


        public async Task StartCheckingLoop(string cfgFilename)
        {
            //_fileName = cfgFilename;

            if (!_cfgFile.ReadFrom(cfgFilename)) return;
            UserSession.SetClient(_d7Client);
            _synchronizer.SetClient(_d7Client);
            _remotes.SetClient(_d7Client, _cfgFile);

            while (true)
            {
                await UpdateNowCmd.ExecuteAsync(null);
                await DelayRetry(_cfgFile.IntervalMins);
            }
        }


        private async Task DelayRetry(int minutes)
        {
            for (int i = minutes; i > 0; i--)
            {
                Trace_n($"Next update check in {i} minutes...", "");
                await Task.Delay(1000 * 60);
            }
        }


        private async Task<bool> UpdateNow(CancellationToken cancelToken)
        {
            var localF = Self.Folder.Path;

            //if (!_cfgFile.ReadFrom(_fileName)) return false;

            if (!await _remotes.LoadAsync(cancelToken,
                                          URL.repo_data_source, 
                                          _cfgFile.AppNid)) return false;

            var groupd = _grouper.GroupFilesByName(localF, _remotes, SyncDirection.Download);

            //foreach (var f in groupd)
            //    Trace_n(f.Filename, $"{f.Comparison.ToString().AlignRight(15)}  =>  {f.NextStep} in {f.Target}");

            //var origExe = Self.Path;
            var origExe = Process.GetCurrentProcess().MainModule.FileName;

            if (!await _synchronizer.Run(_cfgFile.AppNid, 
                groupd, localF, cancelToken, URL.file_content_x)) return false;

            if (_synchronizer.HasReplacement)
                RestartRequested?.Invoke(this, new EArg<string> { Value = origExe });

            return true;
        }


        //private bool ReadConfigFile(string folderPath)
        //{
            //_cfgFile.CertSelfSigned += (s, e) => 
            //{
            //    X509.AddCertificate("Base64-Encoded-X509.cer", this);
            //    //Trace_n("Certificate is self-signed.", $"Allowing from {e.Url}...");
            //    //Ssl.AllowSelfSignedFrom(e.Url);
            //};

            //return _cfgFile.ReadFrom<ConfigFileDto>(folderPath);
        //}
    }
}