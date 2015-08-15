using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using ErrH.Tools.ErrorConstructors;
using ErrH.Tools.Loggers;
using ErrH.WinTools;

namespace ErrH.ConsoleCtrlShim
{
    public partial class TwoColumnLogCtrl : UserControl, ITwoColumnLogView
    {
        private static ConditionalWeakTable<LogEventArg, object> _evtHistory 
            = new ConditionalWeakTable<LogEventArg, object>();


        public TwoColumnLogCtrl()
        {
            InitializeComponent();
            cons.Dock = DockStyle.Fill;
            cons.Stylize(Colour.PuttyPurple);
        }


        public void ShowLog(object src, LogEventArg e)
        {
            if (Fired(e)) return;

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



        //public void ScrollToEnd() => cons.ScrollToEnd();


        private static bool Fired(LogEventArg e, object o = null)
        {
            if (_evtHistory.TryGetValue(e, out o)) return true;
            _evtHistory.Add(e, null);
            return false;
        }



        public void LogHeader(L4j level, string title, string subTitle)
        {
            cons.BlankLine();
            cons.Write2Cols(ColorFor(level), title, subTitle);
        }


        public void LogIntro(L4j level, string text)
        {
            cons.WriteCol1of2(ColorFor(level), text);
        }


        public void LogOutro(L4j level, string text)
        {
            cons.WriteLine(ColorFor(level), text);
        }


        public void LogNormal(L4j level, string col1, string col2)
        {
            if (!level.Polarity()) cons.BlankLine();

            cons.Write2Cols(ColorFor(level), col1, col2);

            if (!level.Polarity()) cons.BlankLine();
        }



        private static Color ColorFor(L4j level)
        {
            switch (level)
            {
                case L4j.Fatal: return Color.Red;
                case L4j.Error: return Color.Red;
                case L4j.Warn: return Color.Yellow;
                case L4j.Info: return Color.White;
                case L4j.Debug: return Color.DarkGray;
                case L4j.Trace: return Color.SlateGray;
                default:
                    throw Error.Unsupported(level, "color map");
            }
        }

    }
}
