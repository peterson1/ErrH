using System.Windows;
using ErrH.Uploader.ViewModels;

namespace ErrH.Uploader.WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                VM.LogAdded += _tabCons.ShowLog;
            };
        }


        private MainWindowVM VM => ((MainWindowVM)DataContext);
    }
}
