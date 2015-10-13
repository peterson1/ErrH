using System.Windows.Media;
using ErrH.Tools.Loggers;

namespace ErrH.WpfTools.LogFormatters
{
    public abstract class TwoColumnLogFormatterBase : LogFormatterBase
    {
        protected abstract Brush   ColorFor       (L4j level);
        protected abstract void    WriteCol1of2   (Brush color, string text);
        protected abstract void    WriteCol2of2   (Brush color, string text);
        protected abstract void    WriteBlankLine ();
        protected abstract string  GetText        ();


        protected override string AppendNormal(L4j level, string title, string message)
        {
            if (!level.Polarity()) WriteBlankLine();
            Write2Cols(ColorFor(level), title, message);
            if (!level.Polarity()) WriteBlankLine();
            return GetText();
        }


        protected override string AppendHeader(L4j level, string title, string subTitle)
        {
            WriteBlankLine();
            Write2Cols(ColorFor(level), title, subTitle);
            return GetText();
        }


        protected override string AppendIntro(L4j level, string text)
        {
            WriteCol1of2(ColorFor(level), text);
            return GetText();
        }


        protected override string AppendOutro(L4j level, string text)
        {
            WriteCol2of2(ColorFor(level), text);
            return GetText();
        }


        protected void Write2Cols (Brush color, string col1, string col2)
        {
            WriteCol1of2(color, col1);
            WriteCol2of2(color, col2);
        }

    }
}
