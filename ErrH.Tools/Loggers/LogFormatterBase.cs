using ErrH.Tools.ErrorConstructors;

namespace ErrH.Tools.Loggers
{
    public abstract class LogFormatterBase : ILogFormatter
    {
        public string RewriteToInclude(LogEventArg e)
        {
            switch (e.ShowAs)
            {
                case ShowLogAs.Normal:
                    return AppendNormal(e.Level, e.Title, e.Message);

                case ShowLogAs.Header:
                    return AppendHeader(e.Level, e.Title, e.Message);
                    
                case ShowLogAs.Intro:
                    return AppendIntro(e.Level, e.Title);
                    
                case ShowLogAs.Outro:
                    return AppendOutro(e.Level, e.Message);
                    
                default:
                    throw Error.Unsupported(e.ShowAs);
            }
        }

        protected abstract string AppendNormal (L4j level, string title, string message);
        protected abstract string AppendHeader (L4j level, string title, string subTitle);
        protected abstract string AppendIntro  (L4j level, string text);
        protected abstract string AppendOutro  (L4j level, string text);
    }
}
