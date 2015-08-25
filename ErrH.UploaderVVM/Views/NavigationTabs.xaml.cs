using System.Windows.Controls;

namespace ErrH.UploaderVVM.Views
{
    /// <summary>
    /// Interaction logic for NavigationTabs.xaml
    /// </summary>
    public partial class NavigationTabs : UserControl
    {
        public NavigationTabs()
        {
            InitializeComponent();

            _appsList.IsEnabledChanged += (s, e) =>
            {
                if (_appsList.IsEnabled
                 && _appsList._listBox.SelectedIndex == -1)
                    _appsList._listBox.SelectedIndex = 0;
            };
        }
    }
}
