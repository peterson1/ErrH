using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;
using ErrH.Core.PCL45.Collections;
using ErrH.Tools.Extensions;
using ErrH.Wpf.net45.Printing;

namespace ErrH.Wpf.net45.Extensions
{
    public static class DataGridExtensions
    {
        public static void AddColHeaderMenuItems(this DataGrid dg, 
            string contxtMenuKey, DataGridLengthUnitType colWidthType = DataGridLengthUnitType.SizeToCells)
        {
            var cm = dg.ColumnHeaderStyle.Resources[contxtMenuKey] as ContextMenu;
            cm.Items.Clear();

            foreach (var col in dg.Columns)
            {
                var mnu = new MenuItem();
                mnu.Header = col.Header;
                mnu.IsCheckable = true;
                mnu.IsChecked = col.Visibility == Visibility.Visible;
                mnu.Click += (s, e) => col.Visibility = mnu.IsChecked
                            ? Visibility.Visible : Visibility.Collapsed;
                cm.Items.Add(mnu);
                col.Width = new DataGridLength(0, colWidthType);
            }

        }


        public static bool AskToPrint(this DataGrid dg, IPrintSpecs printSpecs)
        {
            var dlg = new PrintDialog();
            dlg.UserPageRangeEnabled = true;
            if (dlg.ShowDialog() == false) return false;

            var pagr = new MatelichDataGridPaginator(dg, dlg,
                printSpecs.HeaderLeftText, printSpecs.HeaderCenterText, 
                printSpecs.HeaderRightText, printSpecs.FooterCenterText,
                printSpecs.Resources);


            if (dlg.PageRangeSelection == PageRangeSelection.AllPages)
                dlg.PrintDocument(pagr, printSpecs.PrintJobTitle);
            else
            {
                var rnge = new PageRangeDocumentPaginator(pagr, dlg.PageRange);
                dlg.PrintDocument(rnge, printSpecs.PrintJobTitle);
            }

            return true;
        }


        public static void SummarizeAt<T>(this DataGrid source,
            DataGrid target, int labelColIndex = 0)
        {
            //await Task.Delay(1000 * 1);

            if (source.ItemsSource == null) return;
            target.Columns.Clear();

            foreach (var col in source.Columns)
            {
                var colClone = Clone(col);
                SetStyle(colClone, labelColIndex);
                target.Columns.Add(colClone);
            }

            target.HeadersVisibility = DataGridHeadersVisibility.None;
            target.BorderThickness = new Thickness(1, 0, 1, 0);

            var srcItems = source.ItemsSource.As<Observables<T>>();
            target.ItemsSource = srcItems.SummaryRow;
        }


        private static void SetStyle(DataGridColumn col, int labelColIndex = 0)
        {
            if (col.CellStyle == null)
                col.CellStyle = new Style(typeof(DataGridCell));

            col.CellStyle.Set(FontWeights.Bold);

            var txtCol = col as DataGridTextColumn;
            if (txtCol != null)
            {
                if (txtCol.ElementStyle == null)
                    txtCol.ElementStyle = new Style(typeof(TextBlock));

                txtCol.ElementStyle.Set(VerticalAlignment.Center);

                if (col.DisplayIndex < labelColIndex)
                {
                    txtCol.ElementStyle.Set(Brushes.Transparent);
                }
                else if (col.DisplayIndex == labelColIndex)
                {
                    txtCol.ElementStyle.Set(TextAlignment.Right);
                    txtCol.ElementStyle.Set(FontStyles.Italic);
                }
            }
        }




        private static DataGridColumn Clone(DataGridColumn orig)
        {
            var xaml = XamlWriter.Save(orig);
            var sRdr = new StringReader(xaml);
            var xRdr = XmlReader.Create(sRdr);
            return (DataGridColumn)XamlReader.Load(xRdr);
        }

    }
}
