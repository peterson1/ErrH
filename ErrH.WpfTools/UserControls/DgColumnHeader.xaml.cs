using System.Data;
using System.Windows.Controls;
using AutoDependencyPropertyMarker;

namespace ErrH.WpfTools.UserControls
{
    [AutoDependencyProperty]
    public partial class DgColumnHeader : UserControl
    {

        public DataGrid AttachTo { get; set; }

        public DgColumnHeader()
        {
            InitializeComponent();
            Loaded += (s, e) => CopyCols(AttachTo);
        }


        private void CopyCols(DataGrid host)
        {
            if (host == null) return;
            var dt = new DataTable();

            for (int j = 0; j < host.Columns.Count; j++)
                dt.Columns.Add("col" + j);

            _myGrid.ItemsSource = dt.DefaultView;

            for (int j = 0; j < host.Columns.Count; j++)
            {
                _myGrid.Columns[j].Header = host.Columns[j].Header;
                _myGrid.Columns[j].Width  = host.Columns[j].Width;
            }
        }
    }
}
