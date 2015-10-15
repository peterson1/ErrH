using ErrH.Tools.Extensions;

namespace ErrH.Tools.Loggers
{
    public class ShortLog
    {
        public static string Format(LogEventArg e, int maxCol1Width = 40)
        {
            return e.Title.AlignLeft(maxCol1Width) 
                    + " : " + e.Message;
        }
    }
}
