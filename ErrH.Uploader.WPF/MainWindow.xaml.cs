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
                ((MainWindowVM)DataContext)
                    .LogAdded += _tabCons.ShowLog;
            };
        }
    }
}
