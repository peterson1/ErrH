using System.Drawing;
using System.Windows.Forms;

namespace ErrH.WinTools.ListBoxTools
{
    public static class DrawItemEventArgsExtensions
    {

        /// <summary>
        /// Displays centered text.
        /// Used in ErrH.UploaderForm.appListCtrl.listBox
        /// </summary>
        /// <param name="e"></param>
        /// <param name="text"></param>
        public static void CenterText(this DrawItemEventArgs e, string text)
        {
            var size = e.Graphics.MeasureString(text, e.Font);
            var x = e.Bounds.Left + (e.Bounds.Width / 2 - size.Width / 2);
            var y = e.Bounds.Top + (e.Bounds.Height / 2 - size.Height / 2);

            e.DrawBackground();
            e.Graphics.DrawString(text, e.Font,
                Brushes.Black, new PointF(x, y));
            e.DrawFocusRectangle();

        }
    }
}
