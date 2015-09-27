using System;
using System.Threading.Tasks;
using System.Windows;
using ErrH.Tools.Extensions;
using ErrH.Tools.InversionOfControl;
using ErrH.WpfTools.Themes;

namespace ErrH.WpfTools.Extensions
{
    public static class ApplicationExtensions
    {
        public static Application SetErrorHandlers(this Application app)
        {
            app.DispatcherUnhandledException
                += (s, e) => { HandleErr("Dispatcher", e.Exception); };

            AppDomain.CurrentDomain.UnhandledException
                += (s, e) => { HandleErr("AppDomain", e.ExceptionObject); };

            TaskScheduler.UnobservedTaskException
                += (s, e) => { HandleErr("TaskScheduler", e.Exception); };

            return app;
        }



        static void HandleErr(string thrower, object exceptionObj)
        {
            var ex = exceptionObj as Exception;
            if (ex == null)
            {
                HandleErr(thrower, new Exception(
                    "Non-exception object thrown: " + exceptionObj.GetType().Name));
                return;
            }
            MessageBox.Show(ex.Details(true, true), $"{thrower} :  Unhandled Exception");
        }


        public static Application SetScopeExpiry
            (this Application app, ITypeResolver resolvr)
        {
            app.Exit += (s, e)
                => { resolvr.EndLifetimeScope(); };

            return app;
        }


        public static void AddDataTemplate<TData, TUiElement>(this Application app)
        {
            var dt = new DataTemplate(typeof(TData));
            dt.VisualTree = new FrameworkElementFactory(typeof(TUiElement));
            var key = new DataTemplateKey(typeof(TData));
            app.Resources.Add(key, dt);
        }


        /// <summary>
        /// Adds resource to app's dictionary.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="dictionaryXamlUri"></param>
        /// <param name="uriKind"></param>
        public static void AddResource(this Application app, 
            string dictionaryXamlUri, UriKind uriKind)
        {
            var dict = new ResourceDictionary();
            dict.Source = new Uri(dictionaryXamlUri, uriKind);
            app.Resources.MergedDictionaries.Add(dict);
        }



        public static Application UseTheme<T>(this Application app) 
            where T : IWpfTheme, new()
        {
            var t = new T();
            var f = $"/{t.ProjectName};component/{t.ThemesFolder}/{t.SubFolder}/";

            foreach (var xaml in t.ResourceFilenames)
                app.AddResource(f + xaml, UriKind.Relative);

            return app;
        }
    }
}
