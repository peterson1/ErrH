using System;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ErrH.Wpf.net45.Printing
{
    public class ScaledVisualPrinter
    {
        public static void AskToPrint( ContentPresenter content
                                     , double printScaleOffset
                                     , string printJobDesc = "Tab Content Visual")
        {
            var dlg = new PrintDialog();
            if (dlg.ShowDialog() != true) return;

            OverrideUserSettings(dlg);

            double scale;
            var pCaps = ScaleToFit1Page(content, dlg, out scale, printScaleOffset);

            dlg.PrintVisual(content, "First Fit to Page WPF Print");

            ResetVisualState(content, pCaps);
        }


        public static void AskToPrint(FrameworkElement ctrl
                                    , string printJobDesc = "Scaled Visual")
        {
            PrintDialog print = new PrintDialog();
            if (print.ShowDialog() != true) return;

            PrintCapabilities capabilities = print.PrintQueue.GetPrintCapabilities(print.PrintTicket);

            double scale = Math.Min(capabilities.PageImageableArea.ExtentWidth / ctrl.ActualWidth,
                                    capabilities.PageImageableArea.ExtentHeight / ctrl.ActualHeight);

            ctrl.LayoutTransform = new ScaleTransform(scale, scale);

            Size sz = new Size(capabilities.PageImageableArea.ExtentWidth, capabilities.PageImageableArea.ExtentHeight);
            ctrl.Measure(sz);
            ((UIElement)ctrl).Arrange(new Rect(new Point(capabilities.PageImageableArea.OriginWidth, capabilities.PageImageableArea.OriginHeight),
                sz));

            ctrl.Focus();

            print.PrintVisual(ctrl, printJobDesc);
        }




        private static PrintCapabilities ScaleToFit1Page(ContentPresenter content,
            PrintDialog dlg, out double scale, double printScaleOffset)
        {
            var prCaps = dlg.PrintQueue.GetPrintCapabilities(dlg.PrintTicket);

            //get scale of the print wrt to screen of WPF visual
            scale = Math.Min(prCaps.PageImageableArea.ExtentWidth
                / content.ActualWidth, prCaps.PageImageableArea.ExtentHeight /
                    content.ActualHeight);

            scale += printScaleOffset;

            //Transform the Visual to scale
            content.LayoutTransform = new ScaleTransform(scale, scale);

            //get the size of the printer page
            Size sz = new Size(prCaps.PageImageableArea.ExtentWidth, prCaps.PageImageableArea.ExtentHeight);

            //update the layout of the visual to the printer page size.
            content.Measure(sz);
            content.Arrange(new Rect(new Point(prCaps.PageImageableArea.OriginWidth, prCaps.PageImageableArea.OriginHeight), sz));

            return prCaps;
        }


        private static void ResetVisualState(ContentPresenter objectToPrint, PrintCapabilities printCaps)
        {
            objectToPrint.Width = double.NaN;
            objectToPrint.UpdateLayout();
            objectToPrint.LayoutTransform = new ScaleTransform(1, 1);
            Size size = new Size(printCaps.PageImageableArea.ExtentWidth,
                                 printCaps.PageImageableArea.ExtentHeight);
            objectToPrint.Measure(size);
            objectToPrint.Arrange(new Rect(new Point(printCaps.PageImageableArea.OriginWidth,
                                  printCaps.PageImageableArea.OriginHeight), size));
        }


        private static void OverrideUserSettings(PrintDialog dlg)
        {
            dlg.PrintTicket.PageResolution = new PageResolution(PageQualitativeResolution.High);
        }
    }
}
