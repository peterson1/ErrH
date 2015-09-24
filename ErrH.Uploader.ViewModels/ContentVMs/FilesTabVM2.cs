using System;
using System.Collections.Generic;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.Extensions;
using ErrH.Tools.Loggers;
using ErrH.Uploader.Core;
using ErrH.Uploader.Core.Models;
using ErrH.Uploader.Core.Nodes;
using ErrH.Uploader.Core.Services;
using ErrH.WpfTools.CollectionShims;
using ErrH.WpfTools.ViewModels;

namespace ErrH.Uploader.ViewModels.ContentVMs
{
    public class FilesTabVM2 : WorkspaceVmBase
    {
        private IRepository<AppFileNode> _remotes;
        private AppFileGrouper _locals;
        private AppFolder _app;


        public VmList<RemoteVsLocalFile> MainList { get; }



        public FilesTabVM2(IRepository<AppFileNode> filesRepo,
                           AppFileGrouper fileGrouper)
        {
            _remotes = ForwardLogs(filesRepo);
            _locals  = ForwardLogs(fileGrouper);
            MainList = new VmList<RemoteVsLocalFile>();
            SetEventHandlers();
        }


        protected async override void OnRefresh()
        {
            IsBusy = true;

            await _remotes.LoadAsync(URL.repo_data_source, _app.Nid);

            var groupd = new List<RemoteVsLocalFile>();
            try
            {
                groupd = _locals.GroupFilesByName(_app, _remotes);
            }
            catch (Exception ex)
            {
                Error_n("Error in: _locals.GroupFilesByName()", ex.Details());
            }

            MainList.Clear();
            groupd.ForEach(x => MainList.Add(x));

            IsBusy = false;
        }


        public override int HashCodeFor(object identifier)
        {
            var key = GetType().Name + identifier.As<AppFolder>().Nid;
            return key.GetHashCode();
        }

        public override void SetIdentifier(object identifier)
        {
            base.SetIdentifier(identifier);
            _app = identifier.As<AppFolder>();
            DisplayName = _app.Alias;
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
