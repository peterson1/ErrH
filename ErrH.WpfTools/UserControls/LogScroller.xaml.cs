using System.Windows.Controls;
using System.Windows.Media;
using ErrH.Tools.ErrorConstructors;
using ErrH.Tools.Loggers;
using ErrH.WpfTools.Extensions;
using ErrH.WpfTools.ViewModels;

namespace ErrH.WpfTools.UserControls
{
    public partial class LogScroller : UserControl
    {

        public LogScroller()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                _rtb.Clear();

                //var cntxt = DataContext as LogScrollerVM;
                //if (cntxt != null)
                //    cntxt.LogSource.LogAdded += ShowLog;
            };
        }


        public void LogHeader(L4j level, string title, string subTitle)
        {
            _rtb.WriteBlankLine();
            _rtb.Write2Cols(ColorFor(level), title, subTitle);
        }


        public void ShowLog(object src, LogEventArg e)
        {
            ShowLog(e);
        }


        public void ShowLog(LogEventArg e)
        {
            switch (e.ShowAs)
            {
                case ShowLogAs.Header:
                    this.LogHeader(e.Level, e.Title, e.Message);
                    break;

                case ShowLogAs.Intro:
                    this.LogIntro(e.Level, e.Title);
                    break;

                case ShowLogAs.Outro:
                    this.LogOutro(e.Level, e.Message);
                    break;

                default:
                    this.LogNormal(e.Level, e.Title, e.Message);
                    break;
            }
        }

        public void LogIntro(L4j level, string text)
            => _rtb.WriteCol1of2(ColorFor(level), text);


        public void LogOutro(L4j level, string text)
            => _rtb.WriteCol2of2(ColorFor(level), text);


        public void LogNormal(L4j level, string col1, string col2)
        {
            if (!level.Polarity()) _rtb.WriteBlankLine();
            _rtb.Write2Cols(ColorFor(level), col1, col2);
            if (!level.Polarity()) _rtb.WriteBlankLine();
        }



        private static Color ColorFor(L4j level)
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
