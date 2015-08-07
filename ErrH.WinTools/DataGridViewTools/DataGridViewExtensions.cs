using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ErrH.Tools.ErrorConstructors;
using ErrH.Tools.Extensions;

namespace ErrH.WinTools.DataGridViewTools
{
    public static class DataGridViewExtensions
    {
        public static void AddMergedColumn(this DataGridView grid,
            string format, params int[] colIndeces)
        {
            var newCol = new DataGridViewColumn(new DataGridViewTextBoxCell());
            grid.Columns.Insert(0, newCol);

            for (int i = 0; i < grid.Rows.Count; i++)
            {
                var args = colIndeces.Select(x =>
                            grid.Rows[i].Cells[x + 1].Value).ToArray();

                grid.Rows[i].Cells[0].Value = format.f(args);
                //grid.Rows[i].Height = grid.Rows[i].GetPreferredHeight(i, DataGridViewAutoSizeRowMode.AllCells, false);
                grid.Rows[i].Height = 80;
            }

            newCol.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        }


        public static void AlternateRows(this DataGridView grid, Color alternateRowColor)
        {
            grid.AlternatingRowsDefaultCellStyle
                = new DataGridViewCellStyle
                    (grid.AlternatingRowsDefaultCellStyle)
                {
                    BackColor = alternateRowColor
                };
        }



        public static void ColStyle(this DataGridView grid,
                                    object columnNameOrIndex,
                                    DataGridViewContentAlignment align = DataGridViewContentAlignment.MiddleCenter,
                                    int width = -1,
                                    Color fgColor = default(Color))
        {
            var col = grid.Col(columnNameOrIndex);
            col.SetAlign(align);
            col.SetWidth(width);
            col.SetForeColor(fgColor);
        }



        public static void SetDefaults(this DataGridView grid,
                                       int rowsHeight = 22,
                                       string fontName = "Verdana",
                                       float emSize = 8,
                                       DataGridViewCellBorderStyle borderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                                       int rowHeadersWidth = 24,
                                       DataGridViewContentAlignment alignAll = DataGridViewContentAlignment.MiddleCenter,
                                       int padTop = 0,
                                       int padRight = 5,
                                       int padBottom = 0,
                                       int padLeft = 5)
        {
            grid.SetRowsHeight(rowsHeight);
            grid.SetFont(fontName, emSize);

            grid.SetAlignAll(alignAll);

            grid.DefaultCellStyle.Padding
                = new Padding(padLeft, padTop, padRight, padBottom);

            grid.CellBorderStyle = borderStyle;
            grid.RowHeadersWidth = rowHeadersWidth;//minimum to show the arrow marker

            //grid.ColumnHeadersDefaultCellStyle.Alignment = AlignMid.Center;
        }


        public static void SetAlignAll(this DataGridView grid, DataGridViewContentAlignment alignment)
        {
            grid.DefaultCellStyle.Alignment = alignment;
            grid.ColumnHeadersDefaultCellStyle.Alignment = alignment;
        }


        public static void SetRowsHeight(this DataGridView grid, int rowsHeight)
        {
            grid.RowTemplate.Height = rowsHeight;
        }


        //public static void CellAlignment(this DataGridView grid, 
        //				DataGridViewContentAlignment firstColAlign, 
        //				DataGridViewContentAlignment otherColsAlign)
        //{
        //	grid.DefaultCellStyle.Alignment = otherColsAlign;
        //	if (grid.ColumnCount != 0) grid.Columns[0].DefaultCellStyle.Alignment = firstColAlign;
        //}



        public static void SetFont(this DataGridView grid, string fontName, float emSize)
        {
            grid.DefaultCellStyle.Font = new Font(fontName, emSize);
        }


        public static void ColWidths(this DataGridView grid,
                params int[] widths)
        {
            if (grid == null || grid.ColumnCount == 0) return;

            grid.AutoSizeColumnsMode = ColsWidth.Content;

            for (int i = 0; i < grid.ColumnCount; i++)
            {
                if (i == widths.Length) return;
                grid.Columns[i].SetWidth(widths[i]);
            }
        }


        //public static void CellAlignments(this DataGridView grid,
        //		params DataGridViewContentAlignment[] alignments)
        //{
        //	if (grid == null || grid.ColumnCount == 0) return;

        //	grid.DefaultCellStyle.Alignment 
        //		= grid.ColumnHeadersDefaultCellStyle.Alignment 
        //			= DataGridViewContentAlignment.MiddleCenter;

        //	for (int i = 0; i < grid.ColumnCount; i++)
        //	{
        //		if (i == alignments.Length) return;
        //		var col = grid.Columns[i];
        //		if (col.Visible)
        //		{
        //			col.DefaultCellStyle.Alignment = alignments[i];
        //			col.HeaderCell.Style.Alignment = alignments[i];
        //		}
        //	}
        //}


        public static void RowSetFontStyle(this DataGridView grid, int rowIndex, FontStyle style)
        {
            if (grid == null || rowIndex < 0) return;
            var row = grid.Rows[rowIndex];

            var font = (row.HasDefaultCellStyle && row.DefaultCellStyle.Font != null)
                     ? row.DefaultCellStyle.Font : grid.DefaultCellStyle.Font;

            row.DefaultCellStyle.Font = new Font(font, style);
        }




        /// <summary>
        /// May not be needed is BindingList is sortable.
        /// from: http://stackoverflow.com/a/3861480/3973863
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="initialSortCol"></param>
        public static void EnableClickSort(this DataGridView grid,
                                           object initialSortCol = null)
        {
            foreach (DataGridViewColumn column in grid.Columns)
                column.SortMode = DataGridViewColumnSortMode.Programmatic;

            grid.ColumnHeaderMouseClick += (s, e)
                => { grid.SortBy(e.ColumnIndex); };

            if (initialSortCol != null) grid.SortBy(initialSortCol);
        }


        /// <summary>
        /// Finds column by index or name.
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="colIndexOrName"></param>
        /// <returns></returns>
        public static DataGridViewColumn Col(this DataGridView grid, object colIndexOrName)
        {
            var i = colIndexOrName.ToString();
            return (i.IsNumeric()) ? grid.Columns[i.ToInt()] : grid.Columns[i];
        }



        public static void SortBy(this DataGridView grid, object colIndexOrName)
        {
            var column = grid.Col(colIndexOrName);
            Throw.IfNull(column, "Column[{0}]".f(colIndexOrName));

            if (column.SortMode != DataGridViewColumnSortMode.Programmatic)
                return;

            var sortGlyph = column.HeaderCell.SortGlyphDirection;
            switch (sortGlyph)
            {
                case SortOrder.None:
                case SortOrder.Ascending:
                    grid.Sort(column, ListSortDirection.Descending);
                    column.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                    break;
                case SortOrder.Descending:
                    grid.Sort(column, ListSortDirection.Ascending);
                    column.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                    break;
            }
            grid.Refresh();
        }




        /// <summary>
        /// “May” work if called before setting the DataSource.
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="height"></param>
        public static void SetHeight(this DataGridView grid, int height)
        {
            grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            grid.AllowUserToResizeRows = false;
            grid.RowTemplate.MinimumHeight = height;
        }


        public static DataGridViewColumn Last(this DataGridViewColumnCollection coll, int offset = 0)
        {
            if (coll == null || coll.Count == 0) return null;
            return coll[(coll.Count - 1) + offset];
        }
    }
}
