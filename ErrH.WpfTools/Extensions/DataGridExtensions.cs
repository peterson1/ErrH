using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace ErrH.WpfTools.Extensions
{
    public static class DataGridExtensions
    {

        //public static void AddColumnHeaderMenu(this DataGrid dg)
        //{
        //    var style = dg.ColumnHeaderStyle;

        //    if (style == null)
        //        style = new Style(typeof(DataGridColumnHeader));

        //    DataGridColumnHeader hdf;
        //    hdf.cont
        //}


        /// <summary>
        /// Without this, triangle glyph in column header won't appear
        /// when grid is programmatically sorted.
        /// </summary>
        /// <param name="grid"></param>
        public static void FixIdleSortGlyph(this DataGrid grid)
        {
            var vs = CollectionViewSource.GetDefaultView(grid.Items);
            vs.CollectionChanged += (s, e) =>
            {
                foreach (var desc in vs.SortDescriptions)
                {
                    //grid.ToolTip += $"  {desc.PropertyName}";
                    var col = grid.Columns.Single(x => x.Header.ToString() == desc.PropertyName);
                    col.SortDirection = desc.Direction;
                }
            };
        }
    }
}
