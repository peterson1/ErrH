using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ErrH.Tools.Extensions;
using ErrH.Uploader.Core.Configuration;
using ErrH.Uploader.ViewModels.NavigationVMs;
using ErrH.WpfTools.Commands;
using ErrH.WpfTools.ViewModels;

namespace ErrH.Uploader.ViewModels
{

    public class MainWindowVM : MainWindowVMBase
    {
        public ICommand UploadChangesCmd { get; private set; }


        public MainWindowVM(IConfigFile cfgFile)
        {
            DisplayName = "ErrH Uploader (2nd attempt)";
            StatusVMs.Add(new LogScrollerVM(this));

            cfgFile.CredentialsReady += (s, e) =>
                { UserSession.Credentials = e.Value; };

            InstantiateCommands();
        }

        private void InstantiateCommands()
        {
            UploadChangesCmd = new RelayCommand(x => { MessageBox.Show("sdf"); });
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
