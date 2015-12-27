using System;
using System.IO;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ErrH.Tools.Extensions;
using ErrH.Tools.Loggers;

namespace ErrH.WpfTools.PrintHelpers
{
    public class BenWalkerPrinter
    {
        public static void AskToPrint( ContentPresenter content
                                     , LogSourceBase logger
                                     , string printJobDesc = "Tab Content Visual")
        {
            var dlg = new PrintDialog();
            dlg.UserPageRangeEnabled = true;
            if (dlg.ShowDialog() != true) return;

            //dlg.PageRange = new PageRange(1);
            dlg.PageRangeSelection = PageRangeSelection.UserPages;
            dlg.PageRange = new PageRange(1);
            dlg.PrintTicket.PageOrientation = PageOrientation.Landscape;
            dlg.PrintTicket.PageResolution = new PageResolution(600, 600, PageQualitativeResolution.High);

            Mouse.OverrideCursor = Cursors.Wait;
            DocumentPaginator pagin8r;
            logger.Info_n("Paginating content for printing . . .", "");

            try {
                pagin8r = GetPaginator(content.As<FrameworkElement>(), dlg);
            }
            catch (Exception ex){ logger.LogError("GetPaginator", ex); return; }

            if (pagin8r == null)
            {
                logger.Error_n("pagin8r == null", "Returned NULL DocumentPaginator.");
                return;
            }

            if (dlg.PageRangeSelection == PageRangeSelection.UserPages)
                pagin8r = new PageRangeDocumentPaginator(pagin8r, dlg.PageRange);

            var pCount = pagin8r.PageCount.x("page");
            logger.Info_n($"Sending {pCount} to printer . . .", "");

            dlg.PrintDocument(pagin8r, printJobDesc);
            logger.Info_n("Print job sent to printer.", $"Expect {pCount}.");
        }



        //  adapted from http://www.codeproject.com/Articles/339416/Printing-large-WPF-UserControls
        //
        private static DocumentPaginator GetPaginator( FrameworkElement objectToPrint
                                                     , PrintDialog dlg
                                                     , double dpiScaling = 600.0
        ){
            var printCaps = dlg.PrintQueue.GetPrintCapabilities(dlg.PrintTicket);
            var dpiScale  = dpiScaling / 96.0;
            var doc       = new FixedDocument();
            try
            {
                // Change the layout of the UI Control to match the width of the printer page
                objectToPrint.Width = printCaps.PageImageableArea.ExtentWidth;
                objectToPrint.UpdateLayout();
                objectToPrint.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                Size size = new Size(printCaps.PageImageableArea.ExtentWidth,
                                     objectToPrint.DesiredSize.Height);
                objectToPrint.Measure(size);
                size = new Size(printCaps.PageImageableArea.ExtentWidth,
                                objectToPrint.DesiredSize.Height);
                objectToPrint.Measure(size);
                objectToPrint.Arrange(new Rect(size));

                // Convert the UI control into a bitmap at x dpi
                double dpiX = dpiScaling;
                double dpiY = dpiScaling;
                RenderTargetBitmap bmp = new RenderTargetBitmap(Convert.ToInt32(
                  printCaps.PageImageableArea.ExtentWidth * dpiScale),
                  Convert.ToInt32(objectToPrint.ActualHeight * dpiScale),
                  dpiX, dpiY, PixelFormats.Pbgra32);
                bmp.Render(objectToPrint);

                // Convert the RenderTargetBitmap into a bitmap we can more readily use
                PngBitmapEncoder png = new PngBitmapEncoder();
                png.Frames.Add(BitmapFrame.Create(bmp));
                System.Drawing.Bitmap bmp2;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    png.Save(memoryStream);
                    bmp2 = new System.Drawing.Bitmap(memoryStream);
                }
                doc.DocumentPaginator.PageSize =
                  new Size(dlg.PrintableAreaWidth, dlg.PrintableAreaHeight);

                // break the bitmap down into pages
                int pageBreak = 0;
                int previousPageBreak = 0;
                int pageHeight =
                    Convert.ToInt32(printCaps.PageImageableArea.ExtentHeight * dpiScale);
                while (pageBreak < bmp2.Height - pageHeight)
                {
                    pageBreak += pageHeight;  // Where we thing the end of the page should be

                    // Keep moving up a row until we find a good place to break the page
                    while (!IsRowGoodBreakingPoint(bmp2, pageBreak))
                        pageBreak--;

                    PageContent pageContent = generatePageContent(bmp2, previousPageBreak,
                      pageBreak, doc.DocumentPaginator.PageSize.Width,
                      doc.DocumentPaginator.PageSize.Height, printCaps);
                    doc.Pages.Add(pageContent);
                    previousPageBreak = pageBreak;
                }

                // Last Page
                PageContent lastPageContent = generatePageContent(bmp2, previousPageBreak,
                  bmp2.Height, doc.DocumentPaginator.PageSize.Width,
                  doc.DocumentPaginator.PageSize.Height, printCaps);
                doc.Pages.Add(lastPageContent);
            }
            finally
            {
                // Scale UI control back to the original so we don't effect what is on the screen 
                objectToPrint.Width = double.NaN;
                objectToPrint.UpdateLayout();
                objectToPrint.LayoutTransform = new ScaleTransform(1, 1);
                Size size = new Size(printCaps.PageImageableArea.ExtentWidth,
                                     printCaps.PageImageableArea.ExtentHeight);
                objectToPrint.Measure(size);
                objectToPrint.Arrange(new Rect(new Point(printCaps.PageImageableArea.OriginWidth,
                                      printCaps.PageImageableArea.OriginHeight), size));
                Mouse.OverrideCursor = null;
            }

            return doc.DocumentPaginator;
        }



        private static PageContent generatePageContent(System.Drawing.Bitmap bmp, int top,
                 int bottom, double pageWidth, double PageHeight,
                 System.Printing.PrintCapabilities capabilities)
        {
            FixedPage printDocumentPage = new FixedPage();
            printDocumentPage.Width = pageWidth;
            printDocumentPage.Height = PageHeight;

            int newImageHeight = bottom - top;
            System.Drawing.Bitmap bmpPage = bmp.Clone(new System.Drawing.Rectangle(0, top,
                   bmp.Width, newImageHeight), System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            // Create a new bitmap for the contents of this page
            Image pageImage = new Image();
            BitmapSource bmpSource =
                System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    bmpPage.GetHbitmap(),
                    IntPtr.Zero,
                    System.Windows.Int32Rect.Empty,
                    BitmapSizeOptions.FromWidthAndHeight(bmp.Width, newImageHeight));

            pageImage.Source = bmpSource;
            pageImage.VerticalAlignment = VerticalAlignment.Top;

            // Place the bitmap on the page
            printDocumentPage.Children.Add(pageImage);

            PageContent pageContent = new PageContent();
            ((System.Windows.Markup.IAddChild)pageContent).AddChild(printDocumentPage);

            FixedPage.SetLeft(pageImage, capabilities.PageImageableArea.OriginWidth);
            FixedPage.SetTop(pageImage, capabilities.PageImageableArea.OriginHeight);

            pageImage.Width = capabilities.PageImageableArea.ExtentWidth;
            pageImage.Height = capabilities.PageImageableArea.ExtentHeight;
            return pageContent;
        }


        private static bool IsRowGoodBreakingPoint(System.Drawing.Bitmap bmp, int row)
        {
            double maxDeviationForEmptyLine = 1627500;
            bool goodBreakingPoint = false;

            if (rowPixelDeviation(bmp, row) < maxDeviationForEmptyLine)
                goodBreakingPoint = true;

            return goodBreakingPoint;
        }



        private static double rowPixelDeviation(System.Drawing.Bitmap bmp, int row)
        {
            int count = 0;
            double total = 0;
            double totalVariance = 0;
            double standardDeviation = 0;
            System.Drawing.Imaging.BitmapData bmpData = bmp.LockBits(new System.Drawing.Rectangle(0, 0,
                   bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, bmp.PixelFormat);
            int stride = bmpData.Stride;
            IntPtr firstPixelInImage = bmpData.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)firstPixelInImage;
                p += stride * row;  // find starting pixel of the specified row
                for (int column = 0; column < bmp.Width; column++)
                {
                    count++; //count the pixels


                    byte blue = p[0];
                    byte green = p[1];
                    byte red = p[3];

                    int pixelValue = System.Drawing.Color.FromArgb(0, red, green, blue).ToArgb();
                    total += pixelValue;
                    double average = total / count;
                    totalVariance += Math.Pow(pixelValue - average, 2);
                    standardDeviation = Math.Sqrt(totalVariance / count);

                    // go to next pixel
                    p += 3;
                }
            }
            bmp.UnlockBits(bmpData);

            return standardDeviation;
        }

    }
}
