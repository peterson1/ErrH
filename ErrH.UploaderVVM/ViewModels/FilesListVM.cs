using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.Extensions;
using ErrH.Tools.FileSystemShims;
using ErrH.Tools.Helpers;
using ErrH.UploaderApp;
using ErrH.UploaderApp.AppFileRepository;
using ErrH.UploaderApp.Models;
using ErrH.UploaderApp.Services;
using ErrH.WpfTools.Commands;
using ErrH.WpfTools.ViewModels;

namespace ErrH.UploaderVVM.ViewModels
{
    public class FilesListVM : ListWorkspaceVMBase<FileDiffVM>
    {
        const string GETTING_MSG = "Getting list of files ...";

        private AppFolder                _app;
        private IFileSystemShim          _fs;
        private IRepository<AppFileNode> _remotes;
        private List<FileShim>           _locals;


        public bool      IsBusy    { get; private set; }
        public string    BusyText  { get; private set; } = GETTING_MSG;
        public ICommand  Cancel    { get; }


        public FilesListVM(IRepository<AppFileNode> filesRepo,
                           IFileSystemShim fsShim)
        {
            _fs = fsShim;

            _remotes = ForwardLogs(filesRepo);
            _remotes.Loaded += OnRemoteLoad;

            _remotes.Loading += (s, e) => {
                BusyText = "Getting list of files ..."; };

            _remotes.Retrying += (s, e) => {
                BusyText = $"Unable to get files list.  (retrying in {e.Value} seconds...)"; };

            Cancel = new RelayCommand(x => CancelLoading());
        }

        private void OnRemoteLoad(object sender, System.EventArgs e)
        {
            RefreshVMList();
            IsBusy = false;
            BusyText = GETTING_MSG;
            SortBy("Compared", ListSortDirection.Descending);
        }

        private void CancelLoading()
        {
            _remotes.Cancel();
            IsBusy = false;
        }


        public override void SetIdentifier(object identifier)
        {
            IsBusy = true;

            base.SetIdentifier(identifier);

            _app = Cast.As<AppFolder>(identifier);
            DisplayName = _app.Alias;

            _locals = _fs.Folder(_app.Path).Files.Declutter();

            _remotes.Load(URL.repo_data_source, _app.Nid);
        }


        public override int HashCodeFor(object identifier)
        {
            var key = GetType().Name + Cast.As<AppFolder>(identifier).Nid;
            return key.GetHashCode();
        }


        //protected override List<FileDiffVM> DefineListItems()
        //    => _remotes.All.Select(x 
        //        => new FileDiffVM(x, null)).ToList();

        protected override List<FileDiffVM> DefineListItems()
        {
            var list = new List<FileDiffVM>();

            foreach (var loc in _locals)
            {
                var rem = _remotes.One(x => x.Name == loc.Name);
                list.Add(new FileDiffVM(rem, loc));
            }


            foreach (var rem in _remotes.All)
            {
                if (!list.Has(x=>x.Name == rem.Name))
                {
                    var loc = _locals.One(x => x.Name == rem.Name);
                    list.Add(new FileDiffVM(rem, loc));
                }
            }

            foreach (var file in list)
                file.RunCompare();

            return list;
        }
    }
}
