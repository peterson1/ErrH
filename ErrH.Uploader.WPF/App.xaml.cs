using System.Windows;
using ErrH.Uploader.ViewModels;
using ErrH.WpfTools.Extensions;
using ErrH.WpfTools.UserControls;
using ErrH.WpfTools.ViewModels;
using static ErrH.Uploader.WPF.IocResolver;

namespace ErrH.Uploader.WPF
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            IoC.StartWPF<MainWindow, MainWindowVM>().Show();

            this.AddDataTemplate<BatchFileRunnerVM, BatchFileRunner>();
        }
    }
}
