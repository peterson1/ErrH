using System.Diagnostics;
using System.Windows;

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


        public static void Relaunch(this Application app)
        {
            var origExe = Process.GetCurrentProcess().MainModule.FileName;
            Process.Start(origExe);
            app.Shutdown();
        }
    }
}
