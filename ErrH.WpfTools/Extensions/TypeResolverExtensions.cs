using System.Windows;
using ErrH.Tools.InversionOfControl;
using ErrH.WpfTools.ViewModels;

namespace ErrH.WpfTools.Extensions
{
    public static class TypeResolverExtensions
    {
        public static Window StartWPF<TWindow, TViewModel>(this ITypeResolver resolvr)
            where TWindow    : Window
            where TViewModel : MainWindowVmBase
        {
            Application.Current
                .SetErrorHandlers()
                .SetScopeExpiry(resolvr);


            resolvr.BeginLifetimeScope();

            var win = resolvr.Resolve<TWindow>();
            var vm  = resolvr.Resolve<TViewModel>();

            vm.IoC = resolvr;
            win.DataContext = vm;

            vm.SetCloseHandler(win)
              .SetLoadCompleteHandler();

            return win;
        }
    }
}
