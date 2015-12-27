using System.Windows.Controls;

namespace ErrH.WpfTools.PrintHelpers
{
    public class DirectVisualPrinter
    {
        public static void AskToPrint( ContentPresenter content
                                     , string printJobDesc = "Tab Content Visual")
        {
            var dlg = new PrintDialog();
            if (dlg.ShowDialog() != true) return;
            dlg.PrintVisual(content, printJobDesc);
        }
    }
}
