using System.Drawing;
using System.Windows.Forms;

namespace ErrH.WinTools.DataGridViewTools
{
    public static class DataGridViewColumnExtensions
    {

        public static void SetAlign(this DataGridViewColumn col, DataGridViewContentAlignment alignment, bool alignHeader = true)
        {
            col.DefaultCellStyle.Alignment = alignment;
            if (alignHeader)
            {
                col.HeaderCell.Style.Alignment = alignment;
            }
        }


        public static void SetForeColor
            (this DataGridViewColumn col, Color color)
        {
            if (color == default(Color)) return;
            //col.DefaultCellStyle.ForeColor = color;

            foreach (DataGridViewRow row in col.DataGridView.Rows)
            {
                row.Cells[col.Index].Style.ForeColor = color;
            }
        }


        public static void SetWidth(this DataGridViewColumn col, int width)
        {
            if (col == null || !col.Visible) return;

            if (width == Width.Fill)
            {
                col.AutoSizeMode = ColWidth.Fill;
            }
            else if (width == Width.Content)
            {
                col.AutoSizeMode = ColWidth.Content;
            }
            else
            {
                col.AutoSizeMode = ColWidth.None;
                col.Width = width;
            }

        }
    }
}
