using System.Collections.Generic;
using System.Threading.Tasks;
using ErrH.Tools.Extensions;
using ErrH.Uploader.Core.Configuration;
using ErrH.Uploader.ViewModels.NavigationVMs;
using ErrH.WpfTools.ViewModels;

namespace ErrH.Uploader.ViewModels
{

    public class MainWindowVM : MainWindowVMBase
    {

        public MainWindowVM(IConfigFile cfgFile)
        {
            DisplayName = "ErrH Uploader (2nd attempt)";
            StatusVMs.Add(new LogScrollerVM(this));

            cfgFile.CredentialsReady += (s, e) =>
                { UserSession.Credentials = e.Value; };
        }


        protected override Task<List<WorkspaceViewModelBase>> CreateVMsList()
        {
            var foldrsTab = ForwardLogs(IoC.Resolve<FoldersTabVM>());
            foldrsTab.ParentWindow = this;
            foldrsTab.Refresh();
            return new List<WorkspaceViewModelBase> { foldrsTab }.ToTask();
        }
    }
}
