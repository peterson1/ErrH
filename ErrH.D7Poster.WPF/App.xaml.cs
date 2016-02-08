using System.Windows;
using ErrH.D7Poster.WPF.Configuration;
using ErrH.D7Poster.WPF.ViewModels;
using ErrH.D7Poster.WPF.Views;
using ErrH.Wpf.net45.Extensions;

namespace ErrH.D7Poster.WPF
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var cfg          = SettingsCfg.Load<SettingsCfg>();
            if (cfg == null)
            {
                this.Shutdown();
                return;
            };

            var view         = new MainWindow();
            var modl         = new MainWindowVM();
            view.DataContext = modl;
            view.Show();
        }

        public App()
        {
            this.SetTemplate<TargetFolderVM, TargetFolderView>();
            this.SetTemplate<TransmittalVM, TransmittalView>();
        }
    }
}
