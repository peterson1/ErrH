using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using ErrH.Tools.ErrorConstructors;
using ErrH.UploaderApp.EventArguments;
using ErrH.UploaderApp.Repositories;
using ErrH.WpfTools.ViewModels;

namespace ErrH.UploaderVVM.ViewModels
{
    public class AllAppFoldersViewModel : WorkspaceViewModelBase
    {
        private AppFoldersRepo _repo;


        public event EventHandler<AppFolderEventArg> AppSelected;


        public ObservableCollection<AppFolderViewModel> 
            AllAppFolders { get; private set; }


        public int TotalSelected 
            => AllAppFolders.Count(x => x.IsSelected);



        public AllAppFoldersViewModel(AppFoldersRepo appFoldersRepo)
        {
            base.DisplayName = "All App Folders";

            _repo = appFoldersRepo;
            _repo.AppFolderAdded += OnAppFolderAddedToRepo;
            this.AllAppFolders = CreateAllAppFolders(_repo);
        }

        private void OnAppFolderAddedToRepo(object sender, UploaderApp.EventArguments.AppFolderEventArg e)
        {
            var vm = new AppFolderViewModel(e.App, _repo);
            this.AllAppFolders.Add(vm);
        }

        private ObservableCollection<AppFolderViewModel> CreateAllAppFolders(AppFoldersRepo repo)
        {
            var all = repo.All.Select(x =>
                new AppFolderViewModel(x, repo)).ToList();

            foreach (var vm in all)
                vm.PropertyChanged += OnAppFolderVmPropertyChanged;

            var oc = new ObservableCollection<AppFolderViewModel>(all);
            oc.CollectionChanged += OnCollectionChanged;
            return oc;
        }



        private void OnCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (AppFolderViewModel vm in e.NewItems)
                    vm.PropertyChanged += OnAppFolderVmPropertyChanged;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (AppFolderViewModel vm in e.OldItems)
                    vm.PropertyChanged -= OnAppFolderVmPropertyChanged;
        }



        private void OnAppFolderVmPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var vm = sender as AppFolderViewModel;
            Throw.IfNull(vm, "Expected sender to be ‹AppFolderViewModel›.");

            if (e.PropertyName != nameof(vm.IsSelected)) return;
            OnPropertyChanged(nameof(TotalSelected));

            if (vm.IsSelected)
                AppSelected(sender, EvtArg.AppDir(vm.Model));
        }



        protected override void OnDispose()
        {
            foreach (var vm in AllAppFolders)
                vm.Dispose();

            AllAppFolders.Clear();
            AllAppFolders.CollectionChanged -= OnCollectionChanged;

            _repo.AppFolderAdded -= OnAppFolderAddedToRepo;
        }
    }
}
