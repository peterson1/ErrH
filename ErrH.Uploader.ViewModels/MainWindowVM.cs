using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ErrH.Tools.Extensions;
using ErrH.Tools.InversionOfControl;
using ErrH.Uploader.Core.Configuration;
using ErrH.Uploader.ViewModels.ContentVMs;
using ErrH.Uploader.ViewModels.NavigationVMs;
using ErrH.WpfTools.Commands;
using ErrH.WpfTools.ViewModels;

namespace ErrH.Uploader.ViewModels
{

    public class MainWindowVM : MainWindowVMBase
    {
        public ICommand UploadChangesCmd { get; private set; }


        public MainWindowVM(IConfigFile cfgFile, ITypeResolver resolvr)
            : base(resolvr)
        {
            DisplayName = "ErrH Uploader (2nd attempt)";
            StatusVMs.Add(new LogScrollerVM(this));

            cfgFile.CredentialsReady += (s, e) =>
                { UserSession.Credentials = e.Value; };

            InstantiateCommands();
        }

        private void InstantiateCommands()
        {
            UploadChangesCmd = new RelayCommand(x =>
            {
                //var f = Cast.As<FoldersTabVM>(MainList.SelectedItem);
                MessageBox.Show("later dude");
            });
        }

        protected override Task<List<WorkspaceVmBase>> CreateVMsList()
        {
            var foldrsTab = ForwardLogs(IoC.Resolve<FoldersTabVM>());

            foldrsTab.ItemPicked += (s, e) =>
                { ShowSingleton<FilesTabVM2>(e.Value.Model, IoC); };

            foldrsTab.Refresh();
            return new List<WorkspaceVmBase> { foldrsTab }.ToTask();
        }
    }
}
