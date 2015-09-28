using System.Threading;
using System.Threading.Tasks;
using ErrH.BinUpdater.Core;
using ErrH.BinUpdater.Core.Configuration;
using ErrH.BinUpdater.Core.DTOs;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.Drupal7Models;
using ErrH.Tools.Extensions;
using ErrH.Tools.FileSynchronization;
using ErrH.WinTools.NetworkTools;
using ErrH.WinTools.ReflectionTools;
using ErrH.WpfTools.Commands;
using ErrH.WpfTools.ViewModels;
using PropertyChanged;

namespace ErrH.BinUpdater.ViewModels
{
    [ImplementPropertyChanged]
    public class BinUpdaterVM : WorkspaceVmBase
    {
        private IConfigFile                     _cfgFile;
        private AppFileGrouper                  _grouper;
        private IFileSynchronizer               _synchronizer;
        private ID7Client                       _d7Client;
        private IRepository<SyncableFileRemote> _remotes;

        public IAsyncCommand  UpdateNowCmd { get; }
        public UserSessionVM  UserSession  { get; }



        public BinUpdaterVM(IRepository<SyncableFileRemote> filesRepo, 
                            AppFileGrouper fileGrouper, 
                            IFileSynchronizer fileSynchronizer, 
                            IConfigFile cfgFile,
                            UserSessionVM usrSessionVm,
                            ID7Client d7Client)
        {
            DisplayName   = "Bin Updater";
            _d7Client     = ForwardLogs(d7Client);
            _grouper      = ForwardLogs(fileGrouper);
            _synchronizer = ForwardLogs(fileSynchronizer);
            _remotes      = ForwardLogs(filesRepo);
            _cfgFile      = ForwardLogs(cfgFile);
            UserSession   = ForwardLogs(usrSessionVm);
            UpdateNowCmd  = AsyncCommand.Create(tkn => UpdateNow(tkn));

            UserSession.SetClient(_d7Client);
            _synchronizer.SetClient(_d7Client);

            _cfgFile.CredentialsReady += (s, e) =>
            {
                UserSession.Credentials = e.Value;
                _remotes.SetClient(_d7Client, e.Value);
            };
        }


        private async Task<bool> UpdateNow(CancellationToken cancelToken)
        {
            await Task.Delay(1000 * 1);
            var localF = Self.Folder.Path;

            if (!ReadConfigFile(localF)) return false;

            if (!await _remotes.LoadAsync(URL.repo_data_source, 
                                _cfgFile.AppNid)) return false;

            var groupd = _grouper.GroupFilesByName(localF, _remotes, SyncDirection.Download);

            foreach (var f in groupd)
                Trace_n(f.Filename, $"{f.Comparison.ToString().AlignRight(15)}  =>  {f.NextStep} in {f.Target}");

            //_remotes.clie

            return await _synchronizer.Run(_cfgFile.AppNid, 
                groupd, localF, cancelToken);
        }


        private bool ReadConfigFile(string folderPath)
        {
            _cfgFile.CertSelfSigned += (s, e)
                => { Ssl.AllowSelfSignedFrom(e.Url); };

            return _cfgFile.ReadFrom<ConfigFileDto>(folderPath);
        }
    }
}
