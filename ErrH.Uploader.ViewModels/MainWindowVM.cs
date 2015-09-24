using System.Linq;
using System.Windows.Input;
using ErrH.Tools.Authentication;
using ErrH.Tools.Extensions;
using ErrH.Uploader.Core.Configuration;
using ErrH.Uploader.Core.Services;
using ErrH.Uploader.ViewModels.ContentVMs;
using ErrH.Uploader.ViewModels.NavigationVMs;
using ErrH.WpfTools.Commands;
using ErrH.WpfTools.ViewModels;

namespace ErrH.Uploader.ViewModels
{

    public class MainWindowVM : MainWindowVmBase
    {
        private FileSynchronizer _synchronizer;

        public ICommand UploadChangesCmd { get; private set; }



        public MainWindowVM(IConfigFile cfgFile, ISessionClient d7Client, FileSynchronizer fileSynchronizer)
        {
            DisplayName   = "ErrH Uploader (2nd attempt)";
            _synchronizer = ForwardLogs(fileSynchronizer);

            OtherTabs.Add(new LogScrollerVM(this));

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

            NaviTabs.Add(foldrsTab);
            foldrsTab.Refresh();
            NaviTabs.SelectOne(0);
        }



        private void InstantiateCommands()
        {
            UploadChangesCmd = new RelayCommand(x =>
            {
                var tab = MainTabs.SelectedItem.As<FilesTabVM2>();
                _synchronizer.Run(tab.MainList.ToList());
            });
        }

    }
}
