using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using AutoDependencyPropertyMarker;
using PropertyChanged;

namespace ErrH.WpfTools.UserControls
{
    [AutoDependencyProperty]
    public partial class DgTotalsRow : UserControl
    {
        private DataTable      _data    = new DataTable();
        private List<decimal?> _totals  = new List<decimal?>();
        //private bool           _enabled = true;
        //private int            _lastCount = -1;

        public DataGrid AttachTo { get; set; }


        public DgTotalsRow()
        {
            InitializeComponent();
            Loaded += (sr, ea) => ListenToChanges(AttachTo);
        }


        private void CopyCols(DataGrid host)
        {
            for (int j = 0; j < host.Columns.Count; j++)
            {
                _data.Columns.Add("col" + j);
                _totals.Add((decimal?)null);
            }

            _dg.ItemsSource = _data.DefaultView;

            for (int j = 0; j < host.Columns.Count; j++)
                _dg.Columns[j].Width = host.Columns[j].Width;
        }



        private void ListenToChanges(DataGrid host)
        {
            if (host == null) return;
            CopyCols(host);

            host.Items.CurrentChanged += (s, e) => ClearTotals();
            host.Sorting += async (s, e) =>
            {
                host.CanUserSortColumns = false;
                await Task.Delay(1000 * 1);
                host.CanUserSortColumns = true;
            };

            host.LoadingRow += async (s, e) =>
            {
                //if (host.Items.Count == _lastCount) return;
                //_lastCount = host.Items.Count;

                await Task.Delay(500);

                for (int j = 0; j < host.Columns.Count; j++)
                {
                    var cell = host.Columns[j].GetCellContent(e.Row);
                    _totals[j] = TryIncrement(_totals[j], cell);
                }

                _data.Rows.Clear();
                var row = _data.NewRow();

                for (int j = 0; j < host.Columns.Count; j++)
                    row[j] = string.Format("{0:n}", _totals[j]);

                AddTotalLabel(row);

                _data.Rows.Add(row);
            };

            //host.Sorting += (s, e) => _enabled = false;

            //host.DataContextChanged += (s, e) =>
            //{
            //    _totals.ForEach(x => x = null);
            //    _enabled = true;
            //};
        }

        private void ClearTotals()
        {
            for (int i = 0; i < _totals.Count; i++)
                _totals[i] = null;
        }

        private void AddTotalLabel(DataRow row)
        {
            row[0] = "total";
        }


        private decimal? TryIncrement(decimal? oldSum, FrameworkElement dgCell)
        {
            var txtBlk = dgCell as TextBlock;
            if (txtBlk == null) return null;

            decimal val;
            if (!decimal.TryParse(txtBlk.Text, out val)) return null;

            return oldSum.HasValue ? oldSum + val : val;
        }

    }


    [ImplementPropertyChanged]
    public class DgTotalCell : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Value { get; set; }

        public DgTotalCell(string value)
        {
            this.Value = value;
        }
    }
}
