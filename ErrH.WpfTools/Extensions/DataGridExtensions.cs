using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace ErrH.WpfTools.Extensions
{
    public static class DataGridExtensions
    {
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
