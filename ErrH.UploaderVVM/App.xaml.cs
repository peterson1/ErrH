using System.Windows;
using ErrH.UploaderVVM.ViewModels;
using ErrH.WpfTools.Extensions;
using static ErrH.UploaderVVM.IocResolver;

namespace ErrH.UploaderVVM
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs args)
        {
            base.OnStartup(args);

            IoC.StartWPF<MainWindow, MainWindowVM>().Show();
        }
    }
}

