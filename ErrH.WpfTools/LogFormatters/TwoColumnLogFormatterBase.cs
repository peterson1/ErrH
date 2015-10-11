using System;
using System.Windows.Media;
using ErrH.Tools.Loggers;

namespace ErrH.WpfTools.LogFormatters
{
    public abstract class TwoColumnLogFormatterBase : LogFormatterBase
    {
        protected abstract Color ColorFor(L4j level);
        protected abstract void Write2Cols(Color color, string col1, string col2);
        protected abstract void WriteBlankLine();
        protected abstract string GetText();
        protected abstract void WriteCol1of2(Color color, string text);
        protected abstract void WriteCol2of2(Color color, string text);


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

    }
}
