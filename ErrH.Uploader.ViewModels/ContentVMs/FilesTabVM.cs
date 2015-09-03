using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.Converters;
using ErrH.Tools.Extensions;
using ErrH.Tools.FileSystemShims;
using ErrH.Tools.Loggers;
using ErrH.Uploader.Core;
using ErrH.Uploader.Core.Models;
using ErrH.Uploader.Core.Nodes;
using ErrH.Uploader.Core.Services;
using ErrH.WpfTools.ViewModels;

namespace ErrH.Uploader.ViewModels.ContentVMs
{
    public class FilesTabVM : ListWorkspaceVMBase<FileDiffVM>
    {
        private AppFolder _app;
        private IFileSystemShim _fs;
        private IRepository<AppFileNode> _remotes;
        private List<FileShim> _locals;


        public FilesTabVM(IRepository<AppFileNode> filesRepo,
                          IFileSystemShim fsShim)
        {
            _fs = fsShim;
            _remotes = ForwardLogs(filesRepo);
            SetStatusMessages();

            Cancelled += (s, e) => { _remotes.FireCancel(); };
        }


        //todo: move all these logic outahere!!

        protected async override Task<List<FileDiffVM>> CreateVMsList()
        {
            var list = new List<FileDiffVM>();
            _locals = _fs.Folder(_app.Path).Files.Declutter();
            await _remotes.LoadAsync(URL.repo_data_source, _app.Nid);

            foreach (var loc in _locals)
            {
                var rem = _remotes.One(x => x.Name == loc.Name);
                list.Add(new FileDiffVM(rem, loc));
            }

            foreach (var rem in _remotes.Any(r
                => !list.Has(l => l.Name == r.Name)))
            {
                var loc = _locals.One(x => x.Name == rem.Name);
                list.Add(new FileDiffVM(rem, loc));
            }

            list?.ForEach(x => x.RunCompare());
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
            SortBy(nameof(FileDiffVM.Compared),
                ListSortDirection.Descending);
        }


        private void SetStatusMessages()
        {
            _remotes.Loading += (s, e) =>
            {
                BusyText = "Getting list of files ...";
                RetryingText = "";
                MessageTone = L4j.Info;
                IsDelayingRetry = false;
            };

            _remotes.DelayingRetry += (s, e) =>
            {
                BusyText = $"Unable to get list of files.";
                RetryingText = $"retrying in {e.Value} ...";
                MessageTone = L4j.Warn;
                IsDelayingRetry = true;
            };

            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(IsDelayingRetry))
                    CancelCommand.Fire_CanExecuteChanged();
            };
        }

    }
}
