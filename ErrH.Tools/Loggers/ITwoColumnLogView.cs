namespace ErrH.Tools.Loggers
{
    public interface ITwoColumnLogView
    {
        //void LogHeader (L4j level, string title, string subTitle);
        //void LogIntro  (L4j level, string text);
        //void LogOutro  (L4j level, string text);
        //void LogNormal (L4j level, string col1, string col2);

        void ShowLog(object src, LogEventArg e);
    }
}
