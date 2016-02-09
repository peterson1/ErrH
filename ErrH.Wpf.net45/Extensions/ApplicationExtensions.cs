using System.Diagnostics;
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
    }
}
