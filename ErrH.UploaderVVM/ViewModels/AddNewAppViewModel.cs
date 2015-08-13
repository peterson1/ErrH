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
    public class AddNewAppViewModel : WorkspaceViewModelBase, IDataErrorInfo
    {
        private AppFoldersRepo  _repo;
        private IFileSystemShim _fsShim;

        public int     Nid    => Model.Nid;
        public string  Alias  => Model.Alias;
        public string  Path   => Model.Path;

        public override string DisplayName 
            => Model?.Alias ?? "Add new App folder";

        public AppFolder  Model        { get; }
        public bool       IsSelected   { get; set; }
        public ICommand   SaveCommand  { get; }


        public AddNewAppViewModel(AppFolder appFolder, 
                                  AppFoldersRepo appFoldersRepo)
        {
            Model       = appFolder;
            _repo       = appFoldersRepo;
            _fsShim     = IoC.Resolve<IFileSystemShim>();
            SaveCommand = new RelayCommand(x => Save(), 
                                           x => CanSave);
        }





        /// <summary>
        /// Saves the AppFolder to the repository.
        /// This method is invoked by the SaveCommand.
        /// </summary>
        public void Save()
        {
            _repo.Add(Model, _fsShim);
            base.OnPropertyChanged(nameof(DisplayName));
        }

        public bool CanSave => DataError.IsBlank(Model);





        string IDataErrorInfo.Error 
            => DataError.Info(Model, _fsShim);

        string IDataErrorInfo.this[string col]
        {
            get {
                // Dirty the commands registered with CommandManager,
                // such as our Save command, so that they are queried
                // to see if they can execute now.
                CommandManager.InvalidateRequerySuggested();

                return DataError.Info(Model, col, _fsShim);
            }
        }
    }
}
