using ErrH.Tools.Drupal7Models;
using ErrH.Uploader.Core.Configuration;
using ErrH.Uploader.ViewModels.ContentVMs;
using ErrH.WinTools.NetworkTools;
using ErrH.WpfTools.ViewModels;
using static ErrH.UploaderVVM.IocResolver;

namespace ErrH.UploaderVVM.ViewModels
{
    public class MainWindowVM : MainWindowVMBase
    {
        private ID7Client _client;

        public SlowFoldersWVM FoldersVM  { get; }
        public string         Username   { get; private set; }
        public bool           IsLoggedIn { get; private set; }


        public MainWindowVM(IConfigFile cfgFile,
                            ID7Client d7Client,
                            SlowFoldersWVM appFoldrsVM)
        {
            DisplayName  = "ErrH Uploader";

            FoldersVM    = ForwardLogs(appFoldrsVM);
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

            FoldersVM.ItemPicked += (s, e) => {
                ShowSingleton<FilesTabVM>(e.Value.Model, IoC); };

            CompletelyLoaded += (s, e) =>
            {
                FoldersVM.Refresh();
                FoldersVM.MainList[0].IsSelected = true;
            };
        }









    }
}
