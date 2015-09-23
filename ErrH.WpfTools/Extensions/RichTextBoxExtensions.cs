using System;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;
using ErrH.Tools.ErrorConstructors;
using ErrH.Tools.Extensions;

namespace ErrH.WpfTools.Extensions
{
    public static class RichTextBoxExtensions
    {
        private const int    MAX_COL1_of2  = 45;
        private const string COL_SEPARATOR = " : ";

        /// <summary>
        /// Appends colored text to document.
        /// </summary>
        /// <param name="rtb"></param>
        /// <param name="color"></param>
        /// <param name="text"></param>
        public static void Append(this RichTextBox rtb,
            Color color, string text)
        {
            var endPos = rtb.Document.ContentEnd;
            var rnge = new TextRange(endPos, endPos);

            rtb.Dispatcher.Invoke(new Action(() => {
                rnge.Text = text;
                rnge.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(color));
                rtb.ScrollToEnd();
            }));
        }


        /// <summary>
        /// Clears all text.
        /// </summary>
        /// <param name="rtb"></param>
        public static void Clear(this RichTextBox rtb)
            => rtb.Document?.Blocks.Clear();


        /// <summary>
        /// Writes array of strings into columns.
        /// </summary>
        /// <param name="rtb"></param>
        /// <param name="color"></param>
        /// <param name="columnTexts"></param>
        public static void WriteColumns(this RichTextBox rtb,
            Color color, params string[] columnTexts)
        {
            switch (columnTexts.Length)
            {
                case 1:
                    rtb.WriteLine(color, columnTexts[0]);
                    break;

                case 2:
                    rtb.Write2Cols(color, columnTexts[0], columnTexts[1]);
                    break;

                default:
                    throw Error.Unsupported(columnTexts.Length, nameof(columnTexts) + ".Length");
            }
        }



        public static void WriteLine(this RichTextBox rtb, Color color, string text)
            => rtb.Append(color, text + L.f);


        public static void WriteCol1of2(this RichTextBox rtb, Color color, string text, int colWidth = MAX_COL1_of2)
            => rtb.Append(color, text.AlignLeft(colWidth, COL_SEPARATOR));


        public static void WriteCol2of2(this RichTextBox rtb, Color color, string text)
            => rtb.WriteLine(color, text);


        public static void WriteBlankLine(this RichTextBox rtb, int lineCount = 1)
            => rtb.Append(Colors.Transparent, L.f.Repeat(lineCount));


        public static void Write2Cols(this RichTextBox rtb,
            Color color, string col1Text, string col2Text, 
            int col1Width = MAX_COL1_of2)
        {
            rtb.WriteCol1of2(color, col1Text, col1Width);
            rtb.WriteCol2of2(color, col2Text);
        }
    }
}
