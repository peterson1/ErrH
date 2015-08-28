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
            DisplayName = "Folders List 2";
            _foldersRepo = ForwardLogs(foldersRepo);
        }


        protected override Task<List<AppFolderVM>> CreateVMsList()
        {
            _foldersRepo.Load(ThisApp.Folder.FullName);

            var all = _foldersRepo.All.Select(x =>
                new AppFolderVM(x, _foldersRepo)).ToList();

            //foreach (var vm in all)
            //    vm.PropertyChanged += OnAppFolderVmPropertyChanged;

            return all.ToTask();
        }



        //private void OnAppFolderVmPropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    var vm = sender as AppFolderVM;
        //    Throw.IfNull(vm, "Expected sender to be ‹AppFolderViewModel›.");

        //    if (e.PropertyName != nameof(vm.IsSelected)) return;

        //    if (vm.IsSelected)
        //        _appSelected?.Invoke(sender, EvtArg.AppDir(vm.Model));
        //}


        private bool CanUploadChanges()
        {
            return true;
        }

        private void UploadChangedFiles()
        {
            MessageBox.Show("whoo hooo!!!");
        }

    }
}
