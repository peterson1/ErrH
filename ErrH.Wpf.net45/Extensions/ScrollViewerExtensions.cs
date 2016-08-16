using System.Windows.Controls;
using System.Windows.Input;

namespace ErrH.Wpf.net45.Extensions
{
    public static class ScrollViewerExtensions
    {
        public static void MakeScrollable(this ScrollViewer scrollr)
        {
            scrollr.PreviewMouseWheel -= Scrollr_PreviewMouseWheel_Handler;
            scrollr.PreviewMouseWheel += Scrollr_PreviewMouseWheel_Handler;
        }

        private static void Scrollr_PreviewMouseWheel_Handler(object sender, MouseWheelEventArgs e)
        {
            var scv = sender as ScrollViewer;
            if (scv == null) return;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }
    }
}
