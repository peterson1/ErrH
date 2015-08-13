using System;
using System.Collections.Generic;
using System.Windows;
using ErrH.Tools.InversionOfControl;
using ErrH.UploaderApp.EventArguments;
using ErrH.UploaderApp.Models;
using ErrH.UploaderApp.Repositories;
using ErrH.WpfTools.ViewModels;
using static ErrH.UploaderVVM.IocResolver;

namespace ErrH.UploaderVVM.ViewModels
{
    public class MainWindowViewModel : MainWindowViewModelBase
    {
        private readonly AppFoldersRepo _repo;


        public AllAppFoldersViewModel AllAppsVM { get; private set; }


        public MainWindowViewModel(AppFoldersRepo appFoldrsRepo)
        {
            DisplayName = "ErrH Uploader";
            _repo = appFoldrsRepo;
            AllAppsVM = IoC.Resolve<AllAppFoldersViewModel>();
            AllAppsVM.AppSelected += (s, e) => {
                ShowSingleton(new FilesListViewModel(e.App, _repo)); };
        }


        protected override List<CommandViewModel> DefineNavigations()
        {
            return new List<CommandViewModel>
            {
                CommandViewModel.Relay("View all Apps", x => 
                    ShowSingleton<AllAppFoldersViewModel>(IoC)),

                CommandViewModel.Relay(
                    "Refresh list of Apps",
                        x => this.RefreshAppsList()),

                CommandViewModel.Relay(
                    "Create new App",
                        x => this.CreateNewFolder())
            };
        }


        private void RefreshAppsList()
        {
            throw new NotImplementedException();
        }


        private void CreateNewFolder()
        {
            var wrkspce = new AddNewAppViewModel(
                new AppFolder(), _repo);

            Workspaces.Add(wrkspce);
            SetActiveWorkspace(wrkspce);
        }


    }
}
