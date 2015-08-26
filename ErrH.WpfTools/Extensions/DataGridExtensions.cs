using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace ErrH.WpfTools.Extensions
{
    public static class DataGridExtensions
    {
        public static void UpdateSortGlyph(this DataGrid grid)
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
