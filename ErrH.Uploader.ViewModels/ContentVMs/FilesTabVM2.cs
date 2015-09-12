using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.Converters;
using ErrH.Tools.Extensions;
using ErrH.Tools.Loggers;
using ErrH.Uploader.Core;
using ErrH.Uploader.Core.Configuration;
using ErrH.Uploader.Core.Models;
using ErrH.Uploader.Core.Nodes;
using ErrH.Uploader.Core.Services;
using ErrH.WpfTools.ViewModels;

namespace ErrH.Uploader.ViewModels.ContentVMs
{
    public class FilesTabVM2 : ListWorkspaceVMBase<FileDiffVM2>
    {
        private IRepository<AppFileNode> _remotes;
        private AppFileGrouper _locals;
        private AppFolder _app;


        public FilesTabVM2(IRepository<AppFileNode> filesRepo,
                           AppFileGrouper fileGrouper)
        {
            _remotes = ForwardLogs(filesRepo);
            _locals  = ForwardLogs(fileGrouper);
            SetEventHandlers();
        }


        protected override async Task<List<FileDiffVM2>> CreateVMsList()
        {
            await _remotes.LoadAsync(URL.repo_data_source, _app.Nid);

            var groupd = new List<RemoteVsLocalFile>();
            try {
                groupd = _locals.GroupFilesByName(_app, _remotes);
            }
            catch (Exception ex)
            {
                Error_n("Error in: _locals.GroupFilesByName()", ex.Details());
            }

            var list   = new List<FileDiffVM2>();

            foreach (var model in groupd)
                list.Add(new FileDiffVM2(model));

            return list;
        }


        public override int HashCodeFor(object identifier)
        {
            var key = GetType().Name + Cast.As<AppFolder>(identifier).Nid;
            return key.GetHashCode();
        }

        public override void SetIdentifier(object identifier)
        {
            base.SetIdentifier(identifier);
            _app = Cast.As<AppFolder>(identifier);
            DisplayName = _app.Alias;
        }

        protected override void SortList()
        {
            SortBy(nameof(FileDiffVM2.Compared), 
                ListSortDirection.Descending);
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
                => { _remotes.FireCancel(); };
        }

    }
}
