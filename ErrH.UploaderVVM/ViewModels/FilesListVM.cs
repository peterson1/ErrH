using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.Extensions;
using ErrH.Tools.FileSystemShims;
using ErrH.Tools.Converters;
using ErrH.UploaderApp;
using ErrH.UploaderApp.AppFileRepository;
using ErrH.UploaderApp.Models;
using ErrH.UploaderApp.Services;
using ErrH.WpfTools.Commands;
using ErrH.WpfTools.ViewModels;
using ErrH.Tools.ScalarEventArgs;

namespace ErrH.UploaderVVM.ViewModels
{
    public class FilesListVM : ListWorkspaceVMBase<FileDiffVM>
    {
        private AppFolder                _app;
        private IFileSystemShim          _fs;
        private IRepository<AppFileNode> _remotes;
        private List<FileShim>           _locals;




        public FilesListVM(IRepository<AppFileNode> filesRepo,
                           IFileSystemShim fsShim)
        {
            _fs      = fsShim;
            _remotes = ForwardLogs(filesRepo);

            RequestRefresh += (s, e) => { ReloadAllFiles(); };
        }



        public ICommand Cancel
            => new RelayCommand(x => {  _remotes.Cancel();
                                        IsBusy = false;  });



        public override void SetIdentifier(object identifier)
        {
            base.SetIdentifier(identifier);
            _app = Cast.As<AppFolder>(identifier);
            DisplayName = _app.Alias;
        }


        private void ReloadAllFiles()
        {
            IsBusy = true;

            _remotes.Loaded   += OnRemoteLoaded;
            _remotes.Loading  += OnRemoteLoading;
            _remotes.Retrying += OnRemoteRetrying;

            _locals = _fs.Folder(_app.Path).Files.Declutter();
            _remotes.Load(URL.repo_data_source, _app.Nid);
        }




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



        public override int HashCodeFor(object identifier)
        {
            var key = GetType().Name + Cast.As<AppFolder>(identifier).Nid;
            return key.GetHashCode();
        }



        private void OnRemoteRetrying(object sender, EArg<int> e)
        {
            BusyText = $"Unable to get files list.  (retrying in {e.Value} seconds...)";
        }

        private void OnRemoteLoading(object sender, EventArgs e)
        {
            BusyText = "Getting list of files ...";
        }

        private void OnRemoteLoaded(object sender, EventArgs e)
        {
            RefreshVMList();
            IsBusy = false;
            SortBy("Compared", ListSortDirection.Descending);
        }




    }
}
