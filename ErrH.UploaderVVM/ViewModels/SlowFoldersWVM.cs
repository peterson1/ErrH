using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.Extensions;
using ErrH.UploaderApp.Models;
using ErrH.WinTools.ReflectionTools;
using ErrH.WpfTools.Commands;
using ErrH.WpfTools.ViewModels;

namespace ErrH.UploaderVVM.ViewModels
{
    public class SlowFoldersWVM : SlowListWvmBase<AppFolderVM>
    {
        private IRepository<AppFolder> _foldersRepo;


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



        public SlowFoldersWVM(IRepository<AppFolder> foldersRepo)
        {
            DisplayName = "Local Folders";
            _foldersRepo = ForwardLogs(foldersRepo);
        }


        protected override Task<List<AppFolderVM>> CreateVMsList()
        {
            _foldersRepo.Load(ThisApp.Folder.FullName);

            var vms = _foldersRepo.All.Select(x =>
                new AppFolderVM(x, _foldersRepo)).ToList();

            //foreach (var vm in all)
            //    vm.PropertyChanged += OnAppFolderVmPropertyChanged;
            //vms.ForEach(v => v.)

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
