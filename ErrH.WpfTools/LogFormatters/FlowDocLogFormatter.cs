using System.Windows.Documents;
using System.Windows.Media;
using ErrH.Tools.ErrorConstructors;
using ErrH.Tools.Loggers;

namespace ErrH.WpfTools.LogFormatters
{
    public class FlowDocLogFormatter : TwoColumnLogFormatterBase
    {
        const int COL1_WIDTH = 45;

        private FlowDocument _doc = new FlowDocument();





        protected override Color ColorFor(L4j level)
        {
            switch (level)
            {
                case L4j.Fatal : return Colors.Red;
                case L4j.Error : return Colors.Red;
                case L4j.Warn  : return Colors.Yellow;
                case L4j.Info  : return Colors.White;
                case L4j.Debug : return Colors.DarkGray;
                case L4j.Trace : return Colors.SlateGray;
                case L4j.Off   : return Colors.Transparent;
                default:
                    throw Error.Unsupported(level, "color map");
            }
        }
    }
}
