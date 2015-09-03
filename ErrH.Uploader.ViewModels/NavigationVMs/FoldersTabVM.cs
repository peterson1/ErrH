using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.Extensions;
using ErrH.Tools.ScalarEventArgs;
using ErrH.Uploader.Core.Models;
using ErrH.Uploader.ViewModels.ContentVMs;
using ErrH.WinTools.ReflectionTools;
using ErrH.WpfTools.Commands;
using ErrH.WpfTools.ViewModels;

namespace ErrH.Uploader.ViewModels.NavigationVMs
{
    public class FoldersTabVM : ListWorkspaceVMBase<FolderVM>
    {
        private IRepository<AppFolder> _repo;

        private ICommand _uploadFilesCmd;
        public  ICommand  UploadFilesCommand
        {
            get
            {
                if (_uploadFilesCmd != null) return _uploadFilesCmd;
                _uploadFilesCmd = new RelayCommand(x => UploadChangedFiles(),
                                                   x => CanUploadChanges());
                return _uploadFilesCmd;
            }
        }



        public FoldersTabVM(IRepository<AppFolder> foldersRepo)
        {
            DisplayName  = "Local Folders";
            _repo = ForwardLogs(foldersRepo);

            ItemPicked += OnItemPicked;
        }


        private void OnItemPicked(object sender, EArg<FolderVM> e)
        {
            //ParentWindow.ShowSingleton<>
        }


        protected override Task<List<FolderVM>> CreateVMsList()
        {
            _repo.Load(ThisApp.Folder.FullName);

            var vms = _repo.Select((f, i) => new FolderVM(f, i));

            return vms.ToTask();
        }


        private bool CanUploadChanges()
        {
            return !IsBusy;
        }

        private void UploadChangedFiles()
        {
            MessageBox.Show("whoo hooo!!!");
        }

    }
}
