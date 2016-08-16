using System.Windows;
using ErrH.Wpf.net45.Printing;

namespace ErrH.Wpf.net45.Extensions
{
    public static class FrameworkElementExtensions
    {
        public static void PrintScaled(this FrameworkElement elm, string printJobDesc = "scaled_printout")
            => ScaledVisualPrinter.AskToPrint(elm, printJobDesc);
    }
}
