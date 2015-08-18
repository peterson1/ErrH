using System;
using System.Threading.Tasks;
using System.Windows;
using ErrH.Tools.Extensions;
using ErrH.Tools.InversionOfControl;
using ErrH.WpfTools.Extensions;
using ErrH.WpfTools.Themes.BasicPlainTheme;
using ErrH.WpfTools.Themes.ErrHBaseTheme;

namespace ErrH.WpfTools
{
    public class WpfShim
    {

        public static void OnStartup(Application app, ITypeResolver resolvr)
        {
            app.DispatcherUnhandledException
                += (s, e) => { HandleErr(e.Exception); };

            AppDomain.CurrentDomain.UnhandledException
                += (s, e) => { HandleErr(e.ExceptionObject); };

            TaskScheduler.UnobservedTaskException
                += (s, e) => { HandleErr(e.Exception); };

            app.Exit += (s, e) 
                => { resolvr.EndLifetimeScope(); };

            app.UseTheme<ErrHBase>();
            app.UseTheme<BasicPlain>();

            resolvr.BeginLifetimeScope();
        }



        static void HandleErr(object exceptionObj)
        {
            var ex = exceptionObj as Exception;
            if (ex == null)
            {
                HandleErr(new Exception(
                    "Non-exception object thrown: " + exceptionObj.GetType().Name));
                return;
            }
            MessageBox.Show(ex.Message(true, true), "Unhandled Exception");
        }
    }
}
