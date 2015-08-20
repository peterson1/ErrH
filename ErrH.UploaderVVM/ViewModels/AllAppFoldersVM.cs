using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.ErrorConstructors;
using ErrH.Tools.ScalarEventArgs;
using ErrH.UploaderApp.EventArguments;
using ErrH.UploaderApp.Models;
using ErrH.WinTools.ReflectionTools;
using ErrH.WpfTools.Commands;
using ErrH.WpfTools.ViewModels;

namespace ErrH.UploaderVVM.ViewModels
{
    public class AllAppFoldersVM : WorkspaceViewModelBase
    {
        public event EventHandler<AppFolderEventArg> AppSelected;


        private IRepository<AppFolder> _repo;


        public ObservableCollection<AppFolderVM> 
            AllAppFolders { get; private set; }



        public AllAppFoldersVM(IRepository<AppFolder> appFoldersRepo)
        {
            base.DisplayName = "All App Folders";

            _repo = appFoldersRepo;
            _repo.Added += OnAppFolderAddedToRepo;
            _repo.Loaded += OnRepoLoad;
        }

        private void OnRepoLoad(object sender, EventArgs e)
        {
            var all = _repo.All.Select(x =>
                new AppFolderVM(x, _repo)).ToList();

            foreach (var vm in all)
                vm.PropertyChanged += OnAppFolderVmPropertyChanged;

            var oc = new ObservableCollection<AppFolderVM>(all);
            oc.CollectionChanged += OnCollectionChanged;

            this.AllAppFolders = oc;
        }

        //public ICommand LoadListCommand => new RelayCommand(c => 
        //{
        //    if (!_repo.Load(ThisApp.Folder.FullName)) return;

        //    //later: move these to _repo.OnLoad
        //});



        private void OnAppFolderAddedToRepo(object sender, EArg<AppFolder> e)
        {
            var vm = new AppFolderVM(e.Value, _repo);
            this.AllAppFolders.Add(vm);
        }




        private void OnCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (AppFolderVM vm in e.NewItems)
                    vm.PropertyChanged += OnAppFolderVmPropertyChanged;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (AppFolderVM vm in e.OldItems)
                    vm.PropertyChanged -= OnAppFolderVmPropertyChanged;
        }



        private void OnAppFolderVmPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var vm = sender as AppFolderVM;
            Throw.IfNull(vm, "Expected sender to be ‹AppFolderViewModel›.");

            if (e.PropertyName != nameof(vm.IsSelected)) return;
            //OnPropertyChanged(nameof(TotalSelected));

            if (vm.IsSelected)
                AppSelected(sender, EvtArg.AppDir(vm.Model));
        }



        protected override void OnDispose()
        {
            if (AllAppFolders == null) return;

            foreach (var vm in AllAppFolders)
                vm.Dispose();

            AllAppFolders.Clear();
            AllAppFolders.CollectionChanged -= OnCollectionChanged;

            _repo.Added -= OnAppFolderAddedToRepo;
        }
    }
}
