using System.Windows;
using ErrH.UploaderVVM.ViewModels;
using ErrH.WpfTools.Extensions;
using ErrH.WpfTools.Themes.BasicPlainTheme;
using ErrH.WpfTools.Themes.ErrHBaseTheme;
using static ErrH.UploaderVVM.IocResolver;

namespace ErrH.UploaderVVM
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs args)
        {
            base.OnStartup(args);

            Application.Current.UseTheme<ErrHBase>()
                               .UseTheme<BasicPlain>();

            IoC.StartWPF<MainWindow, MainWindowVM>().Show();
        }
    }
}

