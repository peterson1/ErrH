using System.Windows;
using System.Windows.Controls;
using ErrH.Tools.Loggers;

namespace ErrH.UploaderVVM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += (s, e) =>
            {
                Row(2).Height = new GridLength(200);
            };

            _uploadBtn.Click += (s, e) =>
            {
                _cons.LogNormal(L4j.Info, "clicked", "upload");
                //Row(0).Height = new GridLength(Row(0).Height.Value + 10);
                //_tabs.Height += 10;
            };
        }


        private GridLength _rememberHeight = GridLength.Auto;



        private RowDefinition Row(int index)
            => _splitGrid.RowDefinitions[index];


        private void Grid_Collapsed(object sender, RoutedEventArgs e)
        {
            var grid = sender as Grid;
            if (grid != null)
            {
                _rememberHeight = Row(2).Height;
                Row(2).Height = GridLength.Auto;
            }
        }

        private void Grid_Expanded(object sender, RoutedEventArgs e)
        {
            var grid = sender as Grid;
            if (grid != null)
            {
                Row(2).Height = _rememberHeight;
            }
        }

    }
}
