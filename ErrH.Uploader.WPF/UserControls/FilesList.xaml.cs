using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ErrH.Uploader.WPF.UserControls
{
    /// <summary>
    /// Interaction logic for FilesList.xaml
    /// </summary>
    public partial class FilesList : UserControl
    {
        public FilesList()
        {
            InitializeComponent();

            //_grid.MouseLeftButtonDown += _grid_MouseLeftButtonDown;
        }

        private void _grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var row = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;
            if (row == null) return;

            row.IsSelected = true;
            //MessageBox.Show("row found");
        }
    }
}
