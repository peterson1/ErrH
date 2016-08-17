using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using ErrH.Tools.Extensions;

namespace ErrH.Wpf.net45.Extensions
{
    public static class ApplicationExtensions
    {
        public static void SetTemplate<TData, TUiElement>(this Application app)
        {
            var dt = new DataTemplate(typeof(TData));
            dt.VisualTree = new FrameworkElementFactory(typeof(TUiElement));
            var key = new DataTemplateKey(typeof(TData));
            app.Resources.Add(key, dt);
        }


        public static void Relaunch(this Application app, string arguments = null)
        {
            var origExe = Process.GetCurrentProcess().MainModule.FileName;

            if (arguments.IsBlank())
                Process.Start(origExe);
            else
                Process.Start(origExe, arguments);

            app.Shutdown();
        }


        public static void AlertAllErrors(this Application app, Action<string> errorLogger = null)
        {
            var msg = "";

            app.DispatcherUnhandledException += (s, e) => 
            {
                msg = ShowAlert("Dispatcher", e.Exception);
                errorLogger?.Invoke(msg);
                e.Handled = true;
            };

            AppDomain.CurrentDomain.UnhandledException += (s, e) => 
            {
                msg = ShowAlert("CurrentDomain", e.ExceptionObject);
                errorLogger?.Invoke(msg);
            };

            TaskScheduler.UnobservedTaskException += (s, e) => 
            {
                msg = ShowAlert("TaskScheduler", e.Exception);
                errorLogger?.Invoke(msg);
            };
        }


        private static string ShowAlert(string thrower, object exceptionObj)
        {
            var shortMsg = ""; var longMsg = "";

            if (exceptionObj == null)
            {
                shortMsg = longMsg = $"NULL exception object received by global handler.";
                goto PreExit;
            }

            var ex = exceptionObj as Exception;
            if (ex == null)
            {
                shortMsg = longMsg = $"Non-exception object thrown: ‹{exceptionObj.GetType().Name}›";
                goto PreExit;
            }

            shortMsg = ex.Details(false, true);
            longMsg  = $"Error from ‹{thrower}›" + L.f + ex.Details(true, true);

            PreExit:
            MessageBox.Show(shortMsg, $"Error from ‹{thrower}›", MessageBoxButton.OK, MessageBoxImage.Error);
            return longMsg;
        }
    }
}
