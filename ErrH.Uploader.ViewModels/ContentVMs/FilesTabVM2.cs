using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.Extensions;
using ErrH.Tools.FileSynchronization;
using ErrH.Tools.Loggers;
using ErrH.Uploader.Core;
using ErrH.Uploader.Core.Services;
using ErrH.WpfTools.CollectionShims;
using ErrH.WpfTools.ViewModels;

namespace ErrH.Uploader.ViewModels.ContentVMs
{
    public class FilesTabVM2 : WorkspaceVmBase
    {
        private AppFileGrouper           _grouper;
        private IFileSynchronizer        _synchronizer;
        private IRepository<SyncableFileRemote> _remotes;


        public SyncableFolderInfo App { get; private set; }
        public VmList<RemoteVsLocalFile> MainList { get; }



        public FilesTabVM2(IRepository<SyncableFileRemote> filesRepo, AppFileGrouper fileGrouper, IFileSynchronizer fileSynchronizer)
        {
            _grouper      = ForwardLogs(fileGrouper);
            _synchronizer = ForwardLogs(fileSynchronizer);
            _remotes      = ForwardLogs(filesRepo);

            MainList      = new VmList<RemoteVsLocalFile>();
            SetEventHandlers();
        }


        public async Task Synchronize()
        {
            IsBusy = true;
            await _synchronizer.Run(App.Nid, 
                                    MainList.ToList(), 
                                    SERVER_DIR.app_files);
            IsBusy = false;
            Refresh();
        }


        protected async override void OnRefresh()
        {
            IsBusy = true;

            await _remotes.LoadAsync(URL.repo_data_source, App.Nid);

            var groupd = new List<RemoteVsLocalFile>();
            try
            {
                groupd = _grouper.GroupFilesByName(App, _remotes);
            }
            catch (Exception ex)
            {
                Error_n("Error in: _grouper.GroupFilesByName()", ex.Details());
            }

            MainList.Clear();
            groupd.ForEach(x => MainList.Add(x));

            IsBusy = false;
        }


        public override int HashCodeFor(object identifier)
        {
            var key = GetType().Name + identifier.As<SyncableFolderInfo>().Nid;
            return key.GetHashCode();
        }

        public override void SetIdentifier(object identifier)
        {
            base.SetIdentifier(identifier);
            App = identifier.As<SyncableFolderInfo>();
            DisplayName = App.Alias;
        }


        private void SetEventHandlers()
        {
            _remotes.Loading += (s, e) =>
            {
                BusyText        = "Getting list of files ...";
                RetryingText    = "";
                MessageTone     = L4j.Info;
                IsDelayingRetry = false;
            };

            _remotes.DelayingRetry += (s, e) =>
            {
                BusyText        = $"Unable to get list of files.";
                RetryingText    = $"retrying in {e.Value} ...";
                MessageTone     = L4j.Warn;
                IsDelayingRetry = true;
            };

            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(IsBusy)
                 || e.PropertyName == nameof(IsDelayingRetry))
                    CancelCommand.Fire_CanExecuteChanged();
            };

            Cancelled += (s, e) 
                => { _remotes.RaiseCancelled(); };
        }

    }
}
