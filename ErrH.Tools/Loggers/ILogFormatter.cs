namespace ErrH.Tools.Loggers
{
    public interface ILogFormatter
    {
        string RewriteToInclude(LogEventArg e);
    }
}
