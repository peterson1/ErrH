using System.Linq;
using ErrH.Tools.Authentication;
using ErrH.Tools.Extensions;
using ErrH.Tools.FileSynchronization;
using ErrH.Uploader.Core;
using ErrH.Uploader.Core.Configuration;
using ErrH.Uploader.ViewModels.ContentVMs;
using ErrH.Uploader.ViewModels.NavigationVMs;
using ErrH.WpfTools.Commands;
using ErrH.WpfTools.ViewModels;

namespace ErrH.Uploader.ViewModels
{

    public class MainWindowVM : MainWindowVmBase
    {
        private IFileSynchronizer _synchronizer;

        public IAsyncCommand UploadChangesCmd { get; private set; }



        public MainWindowVM(IConfigFile cfgFile, ISessionClient d7Client, IFileSynchronizer fileSynchronizer)
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
            UploadChangesCmd = new AsyncCommand(async () =>
            {
                var tab  = MainTabs.SelectedItem.As<FilesTabVM2>();
                var nid  = tab.App.Nid;
                var list = tab.MainList.ToList();
                var dir  = SERVER_DIR.app_files;
                await _synchronizer.Run(nid, list, dir);
                tab.Refresh();
            });
        }

    }
}
