using System.Collections.Specialized;
using System.Data;
using System.Windows.Controls;
using System.Windows.Data;
using AutoDependencyPropertyMarker;
using ErrH.Tools.Extensions;

namespace ErrH.WpfTools.UserControls
{
    [AutoDependencyProperty]
    public partial class DgTotalsRow : UserControl
    {
        private DataTable _data = new DataTable();


        public DataGrid  AttachTo { get; set; }



        public DgTotalsRow()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                AttachTo.Loaded += (t, f) =>
                {
                    CopyCols(AttachTo);
                    ListenToChanges(AttachTo);
                };
            };
        }


        private void CopyCols(DataGrid host)
        {
            _dg.Columns.Clear();
            _data.Columns.Clear();

            foreach (var hostCol in host.Columns)
            {
                _dg.Columns.Add(new DataGridTextColumn
                            { Width = hostCol.Width });

                _data.Columns.Add();
            }
            _dg.DataContext = _data;
        }



        private void ListenToChanges(DataGrid host)
        {
            var colxnVw = (CollectionView)CollectionViewSource.GetDefaultView(host.Items);
            ((INotifyCollectionChanged)colxnVw).CollectionChanged += (s, e) =>
            {
                var row = new object[host.Columns.Count - 1];

                for (int j = 0; j < host.Columns.Count; j++)
                {
                    var sum = TrySumOf(j, host);
                    if (sum.HasValue) row[j] = sum.Value;
                }

                _data.Clear();
                _data.Rows.Add(row);
            };
        }

        private decimal? TrySumOf(int j, DataGrid host)
        {
            decimal? sum = (decimal?)null;

            foreach (DataRowView row in host.Items)
            {
                var cell = row[j].ToString();

                if (cell.IsNumeric())
                {
                    if (sum == null) sum = 0;
                    sum += cell.ToDec();
                }
            }

            return sum;
        }
    }
}
