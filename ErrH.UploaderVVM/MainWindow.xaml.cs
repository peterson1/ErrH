using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using ErrH.UploaderVVM.ViewModels;

namespace ErrH.UploaderVVM
{
    public partial class MainWindow : Window
    {
        private GridLength _rememberHeight = GridLength.Auto;


        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += (s, e) =>
            {
                VM.LogAdded        += _cons.ShowLog;
                VM.PropertyChanged += OnPropertyChanged;

                Resources.Add("_userBlockWidth", _usrBlock.ActualWidth);
            };
        }


        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(VM.Username))
            {
                _usrBlock.Username = VM.Username;
                Resources["_userBlockWidth"] = _usrBlock.ActualWidth;
            }
        }



        private MainWindowVM VM 
            => ((MainWindowVM)DataContext);

        private RowDefinition Row(int index)
            => _contentGrid.RowDefinitions[index];

        private ColumnDefinition Col(int index)
            => _naviGrid.ColumnDefinitions[index];



        private void Grid_Collapsed(object sender, RoutedEventArgs e)
        {
            _rememberHeight = Row(2).Height;
            Row(2).Height = GridLength.Auto;
        }

        private void Grid_Expanded(object sender, RoutedEventArgs e)
        {
            Row(2).Height = _rememberHeight;
        }

    }
}
