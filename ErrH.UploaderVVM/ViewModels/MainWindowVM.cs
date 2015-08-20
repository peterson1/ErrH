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
        private readonly IFoldersRepo _repo;

        public string Username { get; set; } = "Logged in as “User Abjfp”";

        public AllAppFoldersVM AllAppsVM { get; private set; }


        public MainWindowVM(IFoldersRepo appFoldrsRepo)
        {
            DisplayName = "ErrH Uploader";
            _repo = ForwardLogs(appFoldrsRepo);

            AllAppsVM = IoC.Resolve<AllAppFoldersVM>();

            AllAppsVM.AppSelected += (s, e) => {
                ShowSingleton(new FilesListViewModel(e.App, 
                    IoC.Resolve<IFilesRepo>())); };

            _repo.CertSelfSigned += (s, e)
                => { Ssl.AllowSelfSignedFrom(e.Url); };

            CompletelyLoaded += (s, e) 
                => { _repo.Load(ThisApp.Folder.FullName); };
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

        

        private void CreateNewFolder()
        {
            var wrkspce = new AppFolderVM(
                new AppFolder(), _repo);

            Workspaces.Add(wrkspce);
            SetActiveWorkspace(wrkspce);
        }


    }
}
