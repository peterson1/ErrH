using ErrH.BinUpdater.Core.Configuration;
using ErrH.Tools.Authentication;
using ErrH.Uploader.ViewModels.ContentVMs;
using ErrH.Uploader.ViewModels.NavigationVMs;
using ErrH.WpfTools.ViewModels;
using PropertyChanged;

namespace ErrH.Uploader.ViewModels
{
    [ImplementPropertyChanged]
    public class MainWindowVM : MainWindowVmBase
    {


        public MainWindowVM(IConfigFile cfgFile, ISessionClient d7Client, LogScrollerVM logScroller)
        {
            DisplayName   = "ErrH Uploader (2nd attempt)";

            //LogScroller.PlainText.Add("sadff");
            OtherTabs.Add(logScroller.ListenTo(this));

            UserSession.SetClient(d7Client);

            cfgFile.CredentialsReady += (s, e) =>
                { UserSession.Credentials = e.Value; };

            CompletelyLoaded += (src, ea) =>
            {
                var foldrsTab = ForwardLogs(IoC.Resolve<FoldersTabVM>());

                foldrsTab.MainList.ItemPicked += (s, e) =>
                    { ShowSingleton<FilesTabVM2>(e.Value, IoC); };

                NaviTabs.Add(foldrsTab);
                NaviTabs.SelectOne(0);
                foldrsTab.Refresh();

                //OtherTabs.Add(BatRunner());
                //OtherTabs.SelectOne(1);
            };
        }


        //protected override void OnRefresh()
        //{
        //    base.OnRefresh();
        //    //NaviTabs.SelectOne(0);
        //}
        private BatchFileRunnerVM BatRunner()
        {
            var vm = IoC.Resolve<BatchFileRunnerVM>();
            
            vm.Run("error-sample.bat");

            return vm;
        }
    }
}
