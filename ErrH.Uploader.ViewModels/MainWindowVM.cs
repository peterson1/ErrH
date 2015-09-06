using System.Windows;
using ErrH.Tools.Loggers;
using ErrH.Uploader.ViewModels.NavigationVMs;
using ErrH.WpfTools.ViewModels;

namespace ErrH.Uploader.ViewModels
{
    
    public class MainWindowVM : MainWindowVMBase
    {
        public FoldersTabVM FoldersTab { get; private set; }

        public MainWindowVM()
        {
            DisplayName = "ErrH Uploader (2nd attempt)";

            CompletelyLoaded += (s, e) =>
            {
                FoldersTab = ForwardLogs(IoC.Resolve<FoldersTabVM>());
                FoldersTab.ParentWindow = this;
                FoldersTab.Refresh();
            };

            //LogAdded += (s, e) =>
            //{
            //    if (e.Level == L4j.Error)
            //        MessageBox.Show(e.Message, e.Title);
            //};
        }
    }
}
