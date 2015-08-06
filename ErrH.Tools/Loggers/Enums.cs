namespace ErrH.Tools.Loggers
{
    /// <summary>
    /// Based on Log4j levels.
    /// </summary>
    public enum L4j
    {
        /// <summary>
        /// The highest possible rank and is intended to turn off logging.
        /// </summary>
        Off,

        /// <summary>
        /// Severe errors that cause premature termination. Expect these to be immediately visible on a status console.
        /// </summary>
        Fatal,
        Error,  //	Other runtime errors or unexpected conditions. Expect these to be immediately visible on a status console.
        Warn,   //	Use of deprecated APIs, poor use of API, 'almost' errors, other runtime situations that are undesirable or unexpected, but not necessarily "wrong". Expect these to be immediately visible on a status console.
        Info,   //	Interesting runtime events (startup/shutdown). Expect these to be immediately visible on a console, so be conservative and keep to a minimum.
        Debug,  //	Detailed information on the flow through the system. Expect these to be written to logs only.
        Trace   //	Most detailed information. Expect these to be written to logs only.
    }


    public static class L4jExtensions
    {
        public static bool Polarity(this L4j level)
        {
            switch (level)
            {
                case L4j.Info:
                case L4j.Debug:
                case L4j.Trace: return true;
                default: return false;
            }
        }
    }


    public enum ShowLogAs
    {
        Normal,
        Header,
        Intro,
        Outro
    }
}
