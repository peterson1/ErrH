using System.Windows;
using ErrH.D7Poster.WPF.Configuration;
using ErrH.D7Poster.WPF.ViewModels;
using ErrH.D7Poster.WPF.Views;
using ErrH.Wpf.net45.Extensions;
using NLog;
using static CommandLine.Parser;
using static ErrH.D7Poster.WPF.Configuration.CmdOptions;

namespace ErrH.D7Poster.WPF
{
    public partial class App : Application
    {
        private static Logger _logr = LogManager.GetCurrentClassLogger();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (!Default.ParseArguments(e.Args, CmdOpt))
                MessageBox.Show(CmdOpt.GetUsage());

            var cfg = SettingsCfg.Load<SettingsCfg>();
            if (cfg == null)
            {
                this.Shutdown();
                return;
            };

            var view         = new MainWindow();
            var modl         = new MainWindowVM();
            view.DataContext = modl;

            if (!CmdOpt.StealthMode) view.Show();

            _logr.Info("App started. (IsVisible={0})", view.IsVisible);
        }

        public App()
        {
            this.SetTemplate<TargetFolderVM, TargetFolderView>();
            this.SetTemplate<TransmittalVM, TransmittalView>();
        }
    }
}
