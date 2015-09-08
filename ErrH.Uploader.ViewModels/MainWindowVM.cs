using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ErrH.Tools.Extensions;
using ErrH.Tools.Randomizers;
using ErrH.Uploader.ViewModels.NavigationVMs;
using ErrH.WpfTools.CollectionShims;
using ErrH.WpfTools.ViewModels;

namespace ErrH.Uploader.ViewModels
{

    public class MainWindowVM : MainWindowVMBase
    {
        public string        Username    { get; set; } = "not logged in";
        public bool          IsLoggedIn  { get; set; }

        public MainWindowVM()
        {
            DisplayName = "ErrH Uploader (2nd attempt)";

            CompletelyLoaded += (s, e) =>
            {
                this.Refresh();
            };

        }

        protected override Task<List<WorkspaceViewModelBase>> CreateVMsList()
        {
            var foldrsTab = ForwardLogs(IoC.Resolve<FoldersTabVM>());
            foldrsTab.ParentWindow = this;
            foldrsTab.Refresh();

            foldrsTab.PropertyChanged += (src, eArg) =>
            {
                var f = new FakeFactory();
                Username = $" {f.Word} {f.Word} {f.Word} {f.Word}";
                IsLoggedIn = true;
            };

            return new List<WorkspaceViewModelBase> { foldrsTab }.ToTask();
        }
    }
}
