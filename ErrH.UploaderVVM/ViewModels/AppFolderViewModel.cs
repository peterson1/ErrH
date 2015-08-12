using System;
using System.ComponentModel;
using System.Windows.Input;
using ErrH.Tools.DataAttributes;
using ErrH.Tools.FileSystemShims;
using ErrH.UploaderApp.Models;
using ErrH.UploaderApp.Repositories;
using ErrH.WpfTools.Commands;
using ErrH.WpfTools.ViewModels;
using PropertyChanged;
using static ErrH.UploaderVVM.IocResolver;

namespace ErrH.UploaderVVM.ViewModels
{
    [ImplementPropertyChanged]
    public class AppFolderViewModel : WorkspaceViewModelBase, IDataErrorInfo
    {
        private AppFolder       _foldr;
        private AppFoldersRepo  _repo;
        private IFileSystemShim _fsShim;


        public int     Nid    => _foldr.Nid;
        public string  Alias  => _foldr.Alias;
        public string  Path   => _foldr.Path;


        public bool      IsSelected   { get; set; }
        public ICommand  SaveCommand  { get; }

        

        public AppFolderViewModel(AppFolder appFolder, 
                                  AppFoldersRepo appFoldersRepo)
        {
            _foldr      = appFolder;
            _repo       = appFoldersRepo;
            _fsShim     = IoC.Resolve<IFileSystemShim>();
            SaveCommand = new RelayCommand(x => Save(), CanSave());
        }





        private void Save()
        {
            throw new NotImplementedException();
        }

        private Predicate<object> CanSave()
        {
            throw new NotImplementedException();
        }





        public override string DisplayName => _foldr?.Alias;

        public string Error => DataError.Info(_foldr, _fsShim);

        public string this[string col] 
            => DataError.Info(_foldr, col, _fsShim);
    }
}
