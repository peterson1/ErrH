using System;
using System.Threading.Tasks;
using System.Windows;
using ErrH.Tools.Extensions;
using ErrH.Tools.InversionOfControl;

namespace ErrH.WpfTools
{
    public class WpfShim
    {
        //public static ILifetimeScopeShim Shim(ITypeResolver resolvr)
        //{
        //    Application.Current.DispatcherUnhandledException
        //        += (s, e) => { HandleErr(e.Exception); };

        //    //Application.Current.Dispatcher.UnhandledException
        //    //    += (s, e) => { HandleErr(e.Exception); };

        //    AppDomain.CurrentDomain.UnhandledException
        //        += (s, e) => { HandleErr(e.ExceptionObject); };

        //    TaskScheduler.UnobservedTaskException
        //        += (s, e) => { HandleErr(e.Exception); };


        //    return resolvr.BeginLifetimeScope();
        //}

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


            var dict = new ResourceDictionary();
            dict.Source = new Uri("/ErrH.WpfTools;component/ResourceDictionaries/Theme1.xaml", UriKind.Relative);
            //app.Resources.Add("Theme1", dict);
            app.Resources.MergedDictionaries.Add(dict);

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
