using System;
using System.Windows;
using System.Windows.Threading;
using ErrH.Tools.CollectionShims;
using ErrH.UploaderApp.Models;
using ErrH.UploaderApp.Repositories;
using ErrH.WinTools.NetworkTools;
using ErrH.WinTools.ReflectionTools;
using ErrH.WpfTools.ViewModels;
using static ErrH.UploaderVVM.IocResolver;

namespace ErrH.UploaderVVM.ViewModels
{
    public class MainWindowVM : MainWindowVMBase
    {

        public AllAppFoldersVM AllAppsVM { get; }


        public MainWindowVM(AllAppFoldersVM appFoldrsVM,
                            IFilesRepo filesRepo)
        {
            DisplayName  = "ErrH Uploader";
            AllAppsVM    = ForwardLogs(appFoldrsVM);
                           ForwardLogs(filesRepo);


            //AllAppsVM.AppSelected += (s, e) => {
            //    ShowSingleton(new FilesListViewModel(e.App, 
            //        IoC.Resolve<IFilesRepo>())); };
            AllAppsVM.AppSelected += (s, e) => {
                ShowSingleton<FilesListVM>(e.App, IoC); };

            CompletelyLoaded += (s, e) 
                => { AllAppsVM.LoadRepo(); };
        }





        //protected override List<CommandViewModel> DefineNavigations()
        //{
        //    return new List<CommandViewModel>
        //    {
        //        CommandViewModel.Relay("View all Apps", x => 
        //            ShowSingleton<AllAppFoldersViewModel>(IoC)),

        //        CommandViewModel.Relay(
        //            "Refresh list of Apps",
        //                x => this.RefreshAppsList()),

        //        CommandViewModel.Relay(
        //            "Create new App",
        //                x => this.CreateNewFolder())
        //    };
        //}

        

        //private void CreateNewFolder()
        //{
        //    var wrkspce = new AppFolderVM(
        //        new AppFolder(), _foldersRepo);

        //    Workspaces.Add(wrkspce);
        //    SetActiveWorkspace(wrkspce);
        //}


    }
}
