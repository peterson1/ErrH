using System.Windows;
using System.Windows.Input;
using ErrH.Tools.Authentication;
using ErrH.Tools.Extensions;
using ErrH.Uploader.Core.Configuration;
using ErrH.Uploader.Core.Models;
using ErrH.Uploader.ViewModels.ContentVMs;
using ErrH.Uploader.ViewModels.NavigationVMs;
using ErrH.WpfTools.Commands;
using ErrH.WpfTools.ViewModels;

namespace ErrH.Uploader.ViewModels
{

    public class MainWindowVM : MainWindowVmBase
    {
        public ICommand UploadChangesCmd { get; private set; }



        public MainWindowVM(IConfigFile cfgFile, ISessionClient d7Client)
        {
            DisplayName = "ErrH Uploader (2nd attempt)";

            StatusVMs.Add(new LogScrollerVM(this));

            UserSession.SetClient(d7Client);

            cfgFile.CredentialsReady += (s, e) =>
                { UserSession.Credentials = e.Value; };

            InstantiateCommands();
        }


        protected override void OnRefresh()
        {
            var foldrsTab = ForwardLogs(IoC.Resolve<FoldersTabVM>());

            foldrsTab.MainList.ItemPicked += (s, e) =>
                { ShowSingleton<FilesTabVM2>(e.Value, IoC); };

            MainList.Add(foldrsTab);
            foldrsTab.Refresh();
            MainList.SelectOne(0);
        }



        private void InstantiateCommands()
        {
            UploadChangesCmd = new RelayCommand(x =>
            {
                var f = MainList.SelectedItem.As<FoldersTabVM>();
                var a = f.MainList.SelectedItem.As<AppFolder>();
                MessageBox.Show(a.Alias);
            });
        }

    }
}
