using System;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ErrH.Tools.Loggers;
using ErrH.WpfTools.Extensions;

namespace ErrH.WpfTools.PrintHelpers
{
    public class ScaledVisualPrinter
    {
        public static void AskToPrint( ContentPresenter content
                                     , LogSourceBase logger
                                     , double printScaleOffset
                                     , string printJobDesc = "Tab Content Visual")
        {
            var dlg = new PrintDialog();
            if (dlg.ShowDialog() != true) return;

            OverrideUserSettings(dlg);

            logger.Info_n("Loading virtualized rows...", "");
            var wasVirtualized = LoadVirtualizedRows(content, 15);

            double scale;
            var pCaps = ScaleToFit1Page(content, dlg, out scale, printScaleOffset);

            dlg.PrintVisual(content, "First Fit to Page WPF Print");
            logger.Info_n("Print job sent to printer.", $"Expect one (1) page.  (scaled: {Math.Round(scale, 2)})");

            ResetVisualState(content, pCaps, wasVirtualized);
        }


        public static void AskToPrint( FrameworkElement ctrl
                                     , string printJobDesc = "Scaled Visual")
        {
            PrintDialog print = new PrintDialog();
            if (print.ShowDialog() != true) return;

            PrintCapabilities capabilities = print.PrintQueue.GetPrintCapabilities(print.PrintTicket);

            double scale = Math.Min(capabilities.PageImageableArea.ExtentWidth / ctrl.ActualWidth,
                                    capabilities.PageImageableArea.ExtentHeight / ctrl.ActualHeight);

            //Transform oldTransform = ctrl.LayoutTransform;

            ctrl.LayoutTransform = new ScaleTransform(scale, scale);

            //Size oldSize = new Size(ctrl.ActualWidth, ctrl.ActualHeight);

            Size sz = new Size(capabilities.PageImageableArea.ExtentWidth, capabilities.PageImageableArea.ExtentHeight);
            ctrl.Measure(sz);
            ((UIElement)ctrl).Arrange(new Rect(new Point(capabilities.PageImageableArea.OriginWidth, capabilities.PageImageableArea.OriginHeight),
                sz));

            ctrl.Focus();

            print.PrintVisual(ctrl, printJobDesc);
            //ctrl.LayoutTransform = oldTransform;

            //ctrl.Measure(oldSize);

            //((UIElement)ctrl).Arrange(new Rect(new Point(0, 0),
            //    oldSize));
        }



        private static bool LoadVirtualizedRows(ContentPresenter content, int rowCount)
        {
            DataGrid dg;
            if (!content.TryFindChild<DataGrid>(out dg)) return false;
            if (dg == null) return false;
            if (dg.Items.Count == 0) return false;

            if (!dg.EnableRowVirtualization) return false;
            dg.EnableRowVirtualization = false;
            dg.EnableColumnVirtualization = false;
            VirtualizingPanel.SetIsVirtualizing(dg, false);

            for (int i = 0; i < dg.Items.Count; i++)
            {
                dg.ScrollIntoView(dg.Items[i]);
                if (i == rowCount) break;
            }
            dg.ScrollIntoView(dg.Items[0]);
            return true;
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


        private static void ResetVisualState(ContentPresenter objectToPrint, PrintCapabilities printCaps, bool wasVirtualized)
        {
            objectToPrint.Width = double.NaN;
            objectToPrint.UpdateLayout();
            objectToPrint.LayoutTransform = new ScaleTransform(1, 1);
            Size size = new Size(printCaps.PageImageableArea.ExtentWidth,
                                 printCaps.PageImageableArea.ExtentHeight);
            objectToPrint.Measure(size);
            objectToPrint.Arrange(new Rect(new Point(printCaps.PageImageableArea.OriginWidth,
                                  printCaps.PageImageableArea.OriginHeight), size));

            if (!wasVirtualized) return;
            var dg = objectToPrint.FindChild<DataGrid>();
            dg.EnableRowVirtualization = true;
            dg.EnableColumnVirtualization = true;
            VirtualizingPanel.SetIsVirtualizing(dg, true);
        }


        private static void OverrideUserSettings(PrintDialog dlg)
        {
            //dlg.PrintTicket.PageOrientation = PageOrientation.Landscape;
            dlg.PrintTicket.PageResolution = new PageResolution(PageQualitativeResolution.High);
        }
    }
}
