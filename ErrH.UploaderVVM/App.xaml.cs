using System;
using System.Windows;
using ErrH.UploaderVVM.ViewModels;
using ErrH.WpfTools;
using static ErrH.UploaderVVM.IocResolver;

namespace ErrH.UploaderVVM
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs args)
        {
            base.OnStartup(args);
            WpfShim.OnStartup(this, IoC);

            var window = IoC.Resolve<MainWindow>();
            var viewModel = IoC.Resolve<MainWindowViewModel>();

            EventHandler handlr = null; handlr = delegate
            {
                viewModel.RequestClose -= handlr;
                window.Close();
            };
            viewModel.RequestClose += handlr;

            window.DataContext = viewModel;
            window.Show();
        }

    }
}

