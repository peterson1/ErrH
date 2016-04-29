using System.Windows;
using System.Windows.Controls;
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
    }
}
