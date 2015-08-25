using ErrH.Configuration;
using ErrH.Tools.Drupal7Models;
using ErrH.WinTools.NetworkTools;
using ErrH.WpfTools.ViewModels;
using static ErrH.UploaderVVM.IocResolver;

namespace ErrH.UploaderVVM.ViewModels
{
    public class MainWindowVM : MainWindowVMBase
    {
        private ID7Client _client;

        public AllAppFoldersVM AllAppsVM  { get; }
        public string          Username   { get; private set; }
        public bool            IsLoggedIn { get; private set; }



        public MainWindowVM(IConfigFile cfgFile,
                            ID7Client d7Client,
                            AllAppFoldersVM appFoldrsVM)
        {
            DisplayName  = "ErrH Uploader";

            AllAppsVM    = ForwardLogs(appFoldrsVM);
            _client      = ForwardLogs(d7Client);
                           ForwardLogs(cfgFile);

            cfgFile.CertSelfSigned += (s, e) 
                => { Ssl.AllowSelfSignedFrom(e.Url); };

            cfgFile.CredentialsReady += _client.LoginUsingCredentials;

            _client.LoggedIn += (s, e) =>
            {
                IsLoggedIn = true;
                Username = e.Name;
            };


            AllAppsVM.AppSelected += (s, e) => {
                ShowSingleton<FilesListVM>(e.App, IoC); };

            CompletelyLoaded += (s, e) 
                => { AllAppsVM.LoadFolders(); };
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
