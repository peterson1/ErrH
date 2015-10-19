using ErrH.Tools.Authentication;
using ErrH.Uploader.DataAccess;
using ErrH.Uploader.DataAccess.Configuration;
using ErrH.Uploader.ViewModels.ContentVMs;
using ErrH.Uploader.ViewModels.NavigationVMs;
using ErrH.WpfTools.ViewModels;
using PropertyChanged;

namespace ErrH.Uploader.ViewModels
{
    [ImplementPropertyChanged]
    public class MainWindowVM : MainWindowVmBase
    {
        //public bool IsCompleteInfo 
        //    => UserSession?.AuthFile?.IsCompleteInfo ?? false;



        public MainWindowVM(ISessionClient d7Client, LogScrollerVM logScroller, UserSessionVM userSessionVM)
            : base(userSessionVM)
        {
            DisplayName   = "ErrH Uploader (2nd attempt)";
            OtherTabs.Add(logScroller.ListenTo(this));

            CompletelyLoaded += (src, ea) =>
            {
                UserSession.SetClient(d7Client);

                var foldrsTab = ForwardLogs(IoC.Resolve<FoldersTabVM>());

                foldrsTab.MainList.ItemPicked += (s, e) =>
                    { ShowSingleton<FilesTabVM2>(e.Value, IoC); };

                NaviTabs.Add(foldrsTab);
                NaviTabs.SelectOne(0);
                foldrsTab.Refresh();

                //OtherTabs.Add(BatRunner());
                //OtherTabs.SelectOne(1);
                //RaisePropertyChanged(nameof(IsCompleteInfo));
                //UserSession.ra
            };
        }


        private BatchFileRunnerVM BatRunner()
        {
            var vm = IoC.Resolve<BatchFileRunnerVM>();
            
            vm.Run("error-sample.bat");

            return vm;
        }
    }
}
