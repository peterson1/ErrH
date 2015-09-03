using ErrH.Uploader.ViewModels.NavigationVMs;
using ErrH.WpfTools.ViewModels;

namespace ErrH.Uploader.ViewModels
{
    
    public class MainWindowVM : MainWindowVMBase
    {
        public FoldersTabVM FoldersTab { get; private set; }


        public MainWindowVM()
        {
            CompletelyLoaded += (s, e) =>
            {
                FoldersTab = IoC.Resolve<FoldersTabVM>();
                FoldersTab.Refresh();
            };
        }
    }
}
