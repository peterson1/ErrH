using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ErrH.BinUpdater.Core;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.Extensions;
using ErrH.Tools.FileSynchronization;
using ErrH.Tools.Loggers;
using ErrH.WpfTools.CollectionShims;
using ErrH.WpfTools.Commands;
using ErrH.WpfTools.ViewModels;

namespace ErrH.Uploader.ViewModels.ContentVMs
{
    public class FilesTabVM2 : WorkspaceVmBase
    {
        private AppFileGrouper                  _grouper;
        private IFileSynchronizer               _synchronizer;
        private IRepository<SyncableFileRemote> _remotes;
        private SyncableFolderInfo              _app;


        public VmList<RemoteVsLocalFile>  MainList  { get; }

        public IAsyncCommand  UploadChangesCmd { get; private set; }
        public string         StatusText       { get; private set; }
        public string         ButtonText       { get; private set; }



        public FilesTabVM2(IRepository<SyncableFileRemote> filesRepo, AppFileGrouper fileGrouper, IFileSynchronizer fileSynchronizer)
        {
            _grouper      = ForwardLogs(fileGrouper);
            _synchronizer = ForwardLogs(fileSynchronizer);
            _remotes      = ForwardLogs(filesRepo);

            MainList      = new VmList<RemoteVsLocalFile>();

            UploadChangesCmd = AsyncCommand.Create(token => UploadChanges(token));
            SetEventHandlers();
        }


        public async Task<bool> UploadChanges(CancellationToken token)
        {
            IsBusy = true;
            var ok =await _synchronizer.Run(_app.Nid, 
                                            MainList.ToList(), 
                                            SERVER_DIR.app_files,
                                            token,
                                            URL.file_content_x);
            IsBusy = false;
            Refresh();
            return ok;
        }


        protected async override void OnRefresh()
        {
            IsBusy = true;

            await _remotes.LoadAsync(URL.repo_data_source, _app.Nid);

            var groupd = new List<RemoteVsLocalFile>();
            try
            {
                groupd = _grouper.GroupFilesByName(_app.Path, _remotes, SyncDirection.Upload);
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
            _app = identifier.As<SyncableFolderInfo>();
            DisplayName = _app.Alias;
        }


        private void SetEventHandlers()
        {
            MainList.CollectionChanged += UpdateStatusTexts;

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


        private void UpdateStatusTexts(object sender, NotifyCollectionChangedEventArgs e)
        {
            var replace = MainList.Count(x => x.NextStep == FileTask.Replace);
            var create  = MainList.Count(x => x.NextStep == FileTask.Create);
            var delete  = MainList.Count(x => x.NextStep == FileTask.Delete);
            var changes = create + replace + delete;

            StatusText = $"{MainList.Count().x("remote file")} found :  {changes.x("change")}";

            var ss = new List<string>();
            if (replace != 0) ss.Add($"Replace {replace.x("file")}");
            if (create  != 0) ss.Add( $"Create { create.x("file")}");
            if (delete  != 0) ss.Add( $"Delete { delete.x("file")}");

            ButtonText = (changes == 0) ? "No action needed"
                       : string.Join("; ", ss)
                       + " in Remote";
        }
    }
}
