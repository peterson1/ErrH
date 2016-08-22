using System;
using System.Diagnostics;
using System.Threading;
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
                msg = VisualizeException("Dispatcher", e.Exception);
                errorLogger?.Invoke(msg);
                e.Handled = true;
            };

            AppDomain.CurrentDomain.UnhandledException += (s, e) => 
            {
                msg = VisualizeException("CurrentDomain", e.ExceptionObject);
                errorLogger?.Invoke(msg);
            };

            TaskScheduler.UnobservedTaskException += (s, e) => 
            {
                msg = VisualizeException("TaskScheduler", e.Exception);
                errorLogger?.Invoke(msg);
            };
        }


        private static string VisualizeException(string thrower, object exceptionObj)
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

            shortMsg = ex.Details(false, false);
            longMsg  = $"Error from ‹{thrower}›" + L.f + ex.Details(true, true);

            PreExit:
            ShowOnNewThread($"Error from ‹{thrower}›", shortMsg);
            return longMsg;
        }


        private static void ShowOnNewThread(string caption, string message)
        {
            new Thread(new ThreadStart(delegate
            {
                MessageBox.Show(message, caption, 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            )).Start();
        }
    }
}
