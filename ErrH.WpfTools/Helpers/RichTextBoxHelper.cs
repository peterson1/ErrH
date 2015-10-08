using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using ErrH.Tools.Extensions;

namespace ErrH.WpfTools.Helpers
{
    //from http://stackoverflow.com/a/2641774
    public class RichTextBoxHelper : DependencyObject
    {

        public static readonly DependencyProperty ContentProperty 
            = DependencyProperty.RegisterAttached("Content", 
                typeof(string), typeof(RichTextBoxHelper), PropMeta());


        public static string GetContent(DependencyObject obj)
            => (string)obj.GetValue(ContentProperty);


        public static void SetContent(DependencyObject obj, string value)
            => obj.SetValue(ContentProperty, value);



        private static void LoadRange(RichTextBox richTextBox, TextRange range)
        {
            MemoryStream buffer = new MemoryStream();
            range.Save(buffer, DataFormats.Rtf);
            var rtf = Encoding.UTF8.GetString(buffer.ToArray());
            SetContent(richTextBox, rtf);
        }


        private static FrameworkPropertyMetadata PropMeta()
        {
            return new FrameworkPropertyMetadata
            {
                BindsTwoWayByDefault = true,
                PropertyChangedCallback = (obj, e) 
                    => { SetRtbDocument(obj); }
            };
        }


        private static void SetRtbDocument(DependencyObject obj)
        {
            var rtb = (RichTextBox)obj;
            var rtf = GetContent(rtb);
            var doc = new FlowDocument();
            var range = new TextRange(doc.ContentStart, doc.ContentEnd);
            LoadRtfToRange(rtf, range);

            rtb.Document = doc;

            range.Changed += (s2, e2) =>
            {
                if (rtb.Document == doc)
                    LoadRange(rtb, range);
            };
        }

        private static void LoadRtfToRange(string rtf, TextRange range)
        {
            if (rtf.IsBlank()) return;

            var byts = Encoding.UTF8.GetBytes(rtf);

            if (byts.Length != 0)
                range.Load(new MemoryStream(byts), DataFormats.Rtf);
        }
    }
}
