using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using ErrH.Tools.ErrorConstructors;
using ErrH.Tools.Extensions;
using ErrH.Tools.Loggers;

namespace ErrH.WpfTools.LogFormatters
{
    public class FlowDocLogFormatter : TwoColumnLogFormatterBase
    {
        const int    COL1_WIDTH = 45;
        const double FONT_SIZE  = 10.667;

        private FlowDocument _doc  = new FlowDocument();
        private FontFamily   _mono = new FontFamily("Consolas");



        protected override void WriteCol1of2(Brush color, string text)
        {
            //var p = new Paragraph();
            //p.TextAlignment = TextAlignment.Left;
            //var r = Styled(text.AlignLeft(COL1_WIDTH), color);
            //p.Inlines.Add(r);
            //_doc.Blocks.Add(p);

            var s = text.AlignLeft(COL1_WIDTH);
            AddBlock(Styled(s, color));
        }


        protected override void WriteCol2of2(Brush color, string text)
        {
            //var p = _doc.Blocks.LastBlock as Paragraph;
            //var r = Styled(text, color);
            //p.Inlines.Add(r);

            var ss = text.SplitByLine();
            if (ss.Count == 0) return;
            AddInline(Styled(ss[0], color));

            for (int i = 1; i < ss.Count - 1; i++)
                AddBlock(Styled(ss[i], color));
        }


        private void AddBlock(Run run)
        {
            var p = new Paragraph();
            p.TextAlignment = TextAlignment.Left;
            p.Inlines.Add(run);
            _doc.Blocks.Add(p);
        }

        private void AddInline(Run run)
        {
            var p = _doc.Blocks.LastBlock as Paragraph;
            p.Inlines.Add(run);
        }


        protected override void WriteBlankLine()
        {
            _doc.Blocks.Add(new Paragraph());
        }


        protected override string GetText()
        {
            var rnge = new TextRange(_doc.ContentStart, _doc.ContentEnd);
            var stream = new MemoryStream();
            rnge.Save(stream, DataFormats.Rtf);
            return UTF8Encoding.UTF8.GetString(stream.ToArray());
        }


        protected override Brush ColorFor(L4j level)
        {
            switch (level)
            {
                case L4j.Fatal : return Brushes.Red;
                case L4j.Error : return Brushes.Red;
                case L4j.Warn  : return Brushes.Yellow;
                case L4j.Info  : return Brushes.White;
                case L4j.Debug : return Brushes.DarkGray;
                case L4j.Trace : return Brushes.SlateGray;
                case L4j.Off   : return Brushes.Transparent;
                default:
                    throw Error.Unsupported(level, "color map");
            }
        }


        private Run Styled(string text, Brush color)
        {
            var r        = new Run(text);
            r.FontFamily = _mono;
            r.Foreground = color;
            r.FontSize   = FONT_SIZE;
            return r;
        }
    }
}
