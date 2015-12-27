using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;

namespace ErrH.WpfTools.PrintHelpers
{
    public class FixedDocPagePrinter
    {
        public static void AskToPrint( ContentPresenter content
                                     , string printJobDesc = "Tab Content Visual")
        {
            var dlg = new PrintDialog();
            if (dlg.ShowDialog() != true) return;
            //dlg.PrintVisual(contentPresenter, "Tab Content Visual");

            var doc = new FixedDocument();
            doc.DocumentPaginator.PageSize 
                = new Size(dlg.PrintableAreaWidth, dlg.PrintableAreaHeight);

            var page = new FixedPage();
            page.Width = doc.DocumentPaginator.PageSize.Width;
            page.Height = doc.DocumentPaginator.PageSize.Height;
            page.Children.Add(content); // <--- error here

            var pgContnt = new PageContent();
            ((IAddChild)pgContnt).AddChild(page);
            doc.Pages.Add(pgContnt);

            dlg.PrintDocument(doc.DocumentPaginator, printJobDesc);
        }
    }
}
