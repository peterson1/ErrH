using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using AutoDependencyPropertyMarker;
using ErrH.WpfTools.CollectionShims;

namespace ErrH.WpfTools.UserControls
{
    [AutoDependencyProperty]
    public partial class DgTotalsRow : UserControl
    {
        public DataGrid       AttachTo { get; set; }
        public DataTableShim  Row1     { get; set; }

        public DgTotalsRow()
        {
            InitializeComponent();
            Loaded += (s, e) => ListenToChanges(AttachTo);
        }




        private void CopyHostCols(DataGrid host, DataGrid targetGrid, DataTable tbl)
        {
            if (host == null || tbl == null) return;

            var colCount = host.Columns.Count;

            for (int j = 0; j < colCount; j++)
                tbl.Columns.Add("col" + j);

            targetGrid.ItemsSource = null;
            targetGrid.ItemsSource = tbl.DefaultView;

            for (int j = 0; j < colCount; j++)
                targetGrid.Columns[j].Width = host.Columns[j].Width;
        }



        private void ListenToChanges(DataGrid host)
        {
            if (host == null) return;
            if (Row1 == null) return;

            Row1.HostCollectionChanged += async (s, e) =>
            {
                if (Row1?.Columns == null 
                 || Row1.Columns.Count == 0)
                    CopyHostCols(host, _grid1, Row1);

                await RecomputeTotals(host, Row1);
            };

            //host.changed += async (s, e) =>
            //{
            //    if (!host.IsVisible) return;
            //    Row1?.Columns?.Clear();
            //    await Task.Delay(1);
            //    Row1?.RaiseHostCollectionChanged(s);
            //};
        }


        private async Task RecomputeTotals(DataGrid host, DataTableShim tbl)
        {
            if (tbl == null) return;
            await Task.Delay(1);

            var colCount = host.Columns.Count;
            var totals   = new List<decimal?>();
            var row      = tbl.NewRow();

            for (int j = 0; j < colCount; j++)
                totals.Add((decimal?)null);

            object firstItem = null;
            foreach (var item in host.ItemsSource)
            {
                if (firstItem == null) firstItem = item;

                if (host.EnableRowVirtualization)
                    host.ScrollIntoView(item);

                for (int j = 0; j < colCount; j++)
                {
                    var cell = host.Columns[j].GetCellContent(item);
                    totals[j] = TryIncrement(j, totals[j], cell);
                }
            }

            for (int j = 0; j < colCount; j++)
            {
                if (totals[j] != null && totals[j].HasValue)
                    row[j] = string.Format("{0:n}", totals[j].Value);
            }
            row[0] = "total";
            tbl?.Rows?.Clear();
            if (row != null) tbl?.Rows?.Add(row);
            
            if (host.EnableRowVirtualization && firstItem != null)
            {
                await Task.Delay(1000 * 2);
                host.ScrollIntoView(firstItem);
            }
        }


        private decimal? TryIncrement(int colIndex, decimal? oldSum, FrameworkElement dgCell)
        {
            var txtBlk = dgCell as TextBlock;
            if (txtBlk == null) return null;

            decimal val;
            if (!decimal.TryParse(txtBlk.Text, out val)) return null;

            return (oldSum != null && oldSum.HasValue) ? oldSum + val : val;
        }
    }
}
