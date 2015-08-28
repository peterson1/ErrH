using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.Converters;
using ErrH.Tools.Extensions;
using ErrH.Tools.FileSystemShims;
using ErrH.UploaderApp;
using ErrH.UploaderApp.AppFileRepository;
using ErrH.UploaderApp.Models;
using ErrH.UploaderApp.Services;
using ErrH.WpfTools.Commands;
using ErrH.WpfTools.ViewModels;

namespace ErrH.UploaderVVM.ViewModels
{
    public class SlowFilesWVM : SlowListWvmBase<FileDiffVM>
    {
        private AppFolder                _app;
        private IFileSystemShim          _fs;
        private IRepository<AppFileNode> _remotes;
        private List<FileShim>           _locals;

        private ICommand _cancel;
        public  ICommand  Cancel
        {
            get
            {
                if (_cancel != null) return _cancel;
                _cancel = new RelayCommand(x =>
                {
                    _remotes.Cancel();
                    IsBusy = false;
                });
                return _cancel;
            }
        }


        public SlowFilesWVM(IRepository<AppFileNode> filesRepo,
                            IFileSystemShim fsShim)
        {
            _fs      = fsShim;
            _remotes = ForwardLogs(filesRepo);
        }


        protected async override Task<List<FileDiffVM>> CreateVMsList()
        {
            _remotes.Loading += (s, e) => {
                BusyText = "Getting list of files ..."; };

            _remotes.Retrying += (s, e) => {
                BusyText = $"Unable to get files list.  (retrying in {e.Value} seconds...)"; };

            _remotes.Loaded += (s, e) => {
                _completion.SetResult(true); };

            _remotes.Load(URL.repo_data_source, _app.Nid);
                await _completion.Task;

            _locals = _fs.Folder(_app.Path).Files.Declutter();

            return CompileAndCompare(_remotes, _locals);
        }


        private List<FileDiffVM> CompileAndCompare(IRepository<AppFileNode> remotes, List<FileShim> locals)
        {
            var list = new List<FileDiffVM>();

            foreach (var loc in locals)
            {
                var rem = remotes.One(x => x.Name == loc.Name);
                list.Add(new FileDiffVM(rem, loc));
            }


            foreach (var rem in remotes.All)
            {
                if (!list.Has(x => x.Name == rem.Name))
                {
                    var loc = locals.One(x => x.Name == rem.Name);
                    list.Add(new FileDiffVM(rem, loc));
                }
            }

            foreach (var file in list)
                file.RunCompare();

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
    }
}
