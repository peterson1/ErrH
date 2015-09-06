using System.Windows;
using ErrH.Tools.InversionOfControl;
using ErrH.WpfTools.Themes.BasicPlainTheme;
using ErrH.WpfTools.Themes.ErrHBaseTheme;
using ErrH.WpfTools.ViewModels;

namespace ErrH.WpfTools.Extensions
{
    public static class TypeResolverExtensions
    {
        public static Window StartWPF<TWindow, TViewModel>(this ITypeResolver resolvr)
            where TWindow    : Window
            where TViewModel : MainWindowVMBase
        {
            Application.Current
                .SetErrorHandlers()
                .SetScopeExpiry(resolvr);
                //.UseTheme<ErrHBase>()
                //.UseTheme<BasicPlain>();


            resolvr.BeginLifetimeScope();

            var win = resolvr.Resolve<TWindow>();
            var vm  = resolvr.Resolve<TViewModel>();

            win.DataContext = vm;

            vm.SetCloseHandler(win)
              .SetLoadCompleteHandler()
              .IoC = resolvr;

            return win;
        }
    }
}
