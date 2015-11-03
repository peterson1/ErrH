using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using AutoDependencyPropertyMarker;

namespace ErrH.WpfTools.UserControls
{
    [AutoDependencyProperty]
    public partial class DgTotalsRow : UserControl
    {
        //private int            _canCompute;
        private int            _actualCount;
        private long           _lastRun;
        private DataTable      _data              = new DataTable();
        //private List<decimal?> _totals            = new List<decimal?>();
        private List<int>      _contributorCounts = new List<int>();

        public DataGrid AttachTo { get; set; }


        public DgTotalsRow()
        {
            InitializeComponent();
            Loaded += (sr, ea) =>
            {
                CopyCols(AttachTo);
                ListenToChanges(AttachTo);
            };
        }




        private void CopyCols(DataGrid host)
        {
            if (host == null) return;

            var colCount = host.Columns.Count;

            for (int j = 0; j < colCount; j++)
            {
                _data.Columns.Add("col" + j);
                //_totals.Add((decimal?)null);
                _contributorCounts.Add(0);
            }

            _dg.ItemsSource = _data.DefaultView;

            for (int j = 0; j < colCount; j++)
                _dg.Columns[j].Width = host.Columns[j].Width;
        }



        private void ListenToChanges(DataGrid host)
        {
            if (host == null) return;

            host.Items.CurrentChanged += async (s, e) =>
            {
                if (TooSoon()) return;
                await Task.Delay(500);
                IncrementTotals(host, host.ItemsSource);
            };
        }

        private bool TooSoon()
        {
            var now    = DateTime.Now.Ticks;
            var elapsd = now - _lastRun;
            _lastRun   = now;

            return elapsd < 10000000;// 10 million ticks = 1 second
        }

        private void IncrementTotals(DataGrid host, IEnumerable items)
        {
            if (items == null) return;

            var colCount = host.Columns.Count;
            var totals = new List<decimal?>();
            for (int j = 0; j < colCount; j++)
            {
                _contributorCounts[j] = 0;
                totals.Add((decimal?)null);
            }

            _actualCount = 0;
            foreach (var item in items)
            {
                host.ScrollIntoView(item);
                for (int j = 0; j < colCount; j++)
                {
                    var cell = host.Columns[j].GetCellContent(item);
                    totals[j] = TryIncrement(j, totals[j], cell);
                }
                _actualCount++;
            }

            _data.Rows.Clear();
            var row = _data.NewRow();

            for (int j = 0; j < colCount; j++)
            {
                if (totals[j] != null && totals[j].HasValue)
                    row[j] = string.Format("{0:n}", totals[j]);
            }

            AddTotalLabel(row);

            _data.Rows.Add(row);
        }


        private void AddTotalLabel(DataRow row)
        {
            row[0] = "total";
            //row[0] = $"{_actualCount} ({string.Join(", ", _contributorCounts)})";
        }

        //private void AddRow(string text)
        //{
        //    var row = _data.NewRow();

        //    for (int j = 1; j < _data.Columns.Count; j++)
        //        row[j] = null;

        //    row[0] = text;

        //    _data.Rows.Add(row);
        //}

        private decimal? TryIncrement(int colIndex, decimal? oldSum, FrameworkElement dgCell)
        {
            var txtBlk = dgCell as TextBlock;
            if (txtBlk == null) return null;

            decimal val;
            if (!decimal.TryParse(txtBlk.Text, out val)) return null;

            _contributorCounts[colIndex] += 1;

            return oldSum.HasValue ? oldSum + val : val;
        }

    }
}
