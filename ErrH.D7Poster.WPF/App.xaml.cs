using System.Windows;
using ErrH.D7Poster.WPF.ViewModels;
using ErrH.D7Poster.WPF.Views;

namespace ErrH.D7Poster.WPF
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var view         = new MainWindow();
            var modl         = new MainWindowVM();
            view.DataContext = modl;
            view.Show();
        }

        public App()
        {
            TemplateFor<TargetFolder, TargetFolderView>();
            TemplateFor<Transmittal, TransmittalView>();
        }


        private void TemplateFor<TData, TUiElement>()
        {
            var dt        = new DataTemplate(typeof(TData));
            dt.VisualTree = new FrameworkElementFactory(typeof(TUiElement));
            var key       = new DataTemplateKey(typeof(TData));
            this.Resources.Add(key, dt);
        }

    }
}
