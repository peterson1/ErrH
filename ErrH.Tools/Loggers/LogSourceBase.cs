using System;
using ErrH.Tools.Extensions;

namespace ErrH.Tools.Loggers
{
    public class LogSourceBase : ILogSource
    {
        //public event EventHandler<LogEventArg> LogAdded = delegate { };

        private      EventHandler<LogEventArg> _logAdded;
        public event EventHandler<LogEventArg>  LogAdded
        {
            add    { _logAdded -= value; _logAdded += value; }
            remove { _logAdded -= value; }
        }


        public L4j   DefaultLevel    { get; set; }
        public bool  IsLogForwarded  { get; set; }


        public bool Fatal_n(string title, object message, params object[] args) { return Arg(L4j.Fatal, ShowLogAs.Normal, title, message.ToString().f(args)); }
        public bool Fatal_h(string title, object subTitle, params object[] args) { return Arg(L4j.Fatal, ShowLogAs.Header, title, subTitle.ToString().f(args)); }
        public bool Fatal_i(string title, params object[] args) { return Arg(L4j.Fatal, ShowLogAs.Intro, title.f(args), ""); }
        public bool Fatal_o(object message, params object[] args) { return Arg(L4j.Fatal, ShowLogAs.Outro, "", message.ToString().f(args)); }

        public T Error_<T>(T returnVal, object titl, object msg, params object[] args) { this.Error_n(titl.ToString(), msg, args); return returnVal; }
        public bool Error_n(string title, object message, params object[] args) { return Arg(L4j.Error, ShowLogAs.Normal, title, message.ToString().f(args)); }
        public bool Error_h(string title, object subTitle, params object[] args) { return Arg(L4j.Error, ShowLogAs.Header, title, subTitle.ToString().f(args)); }
        public bool Error_i(string title, params object[] args) { return Arg(L4j.Error, ShowLogAs.Intro, title.f(args), ""); }
        public bool Error_o(object message, params object[] args) { return Arg(L4j.Error, ShowLogAs.Outro, "", message.ToString().f(args)); }
        public T Error_o_<T>(T returnVal, object message, params object[] args) { this.Error_o(message, args); return returnVal; }

        public T Warn_<T>(T returnVal, object titl, object msg, params object[] args) { this.Warn_n(titl.ToString(), msg, args); return returnVal; }
        public bool Warn_n(string title, object message, params object[] args) { return Arg(L4j.Warn, ShowLogAs.Normal, title, message.ToString().f(args)); }
        public bool Warn_h(string title, object subTitle, params object[] args) { return Arg(L4j.Warn, ShowLogAs.Header, title, subTitle.ToString().f(args)); }
        public bool Warn_i(string title, params object[] args) { return Arg(L4j.Warn, ShowLogAs.Intro, title.f(args), ""); }
        public bool Warn_o(object message, params object[] args) { return Arg(L4j.Warn, ShowLogAs.Outro, "", message.ToString().f(args)); }
        public T Warn_o_<T>(T returnVal, object message, params object[] args) { this.Warn_o(message, args); return returnVal; }

        public T Info_<T>(T returnVal, object titl, object msg, params object[] args) { this.Info_n(titl.ToString(), msg, args); return returnVal; }
        public bool Info_n(string title, object message, params object[] args) { return Arg(L4j.Info, ShowLogAs.Normal, title, message.ToString().f(args)); }
        public bool Info_h(string title, object subTitle, params object[] args) { return Arg(L4j.Info, ShowLogAs.Header, title, subTitle.ToString().f(args)); }
        public bool Info_i(string title, params object[] args) { return Arg(L4j.Info, ShowLogAs.Intro, title.f(args), ""); }
        public bool Info_o(object message, params object[] args) { return Arg(L4j.Info, ShowLogAs.Outro, "", message.ToString().f(args)); }

        public T Debug_<T>(T returnVal, object titl, object msg, params object[] args) { this.Debug_n(titl.ToString(), msg, args); return returnVal; }
        public bool Debug_n(string title, object message, params object[] args) { return Arg(L4j.Debug, ShowLogAs.Normal, title, message.ToString().f(args)); }
        public bool Debug_h(string title, object subTitle, params object[] args) { return Arg(L4j.Debug, ShowLogAs.Header, title, subTitle.ToString().f(args)); }
        public bool Debug_i(string title, params object[] args) { return Arg(L4j.Debug, ShowLogAs.Intro, title.f(args), ""); }
        public bool Debug_o(object message, params object[] args) { return Arg(L4j.Debug, ShowLogAs.Outro, "", message.ToString().f(args)); }

        public T Trace_<T>(T returnVal, object titl, object msg, params object[] args) { this.Trace_n(titl.ToString(), msg, args); return returnVal; }
        public bool Trace_n(string title, object message, params object[] args) { return Arg(L4j.Trace, ShowLogAs.Normal, title, message.ToString().f(args)); }
        public bool Trace_h(string title, object subTitle, params object[] args) { return Arg(L4j.Trace, ShowLogAs.Header, title, subTitle.ToString().f(args)); }
        public bool Trace_i(string title, params object[] args) { return Arg(L4j.Trace, ShowLogAs.Intro, title.f(args), ""); }
        public bool Trace_o(object message, params object[] args) { return Arg(L4j.Trace, ShowLogAs.Outro, "", message.ToString().f(args)); }
        public T Trace_o_<T>(T returnVal, object message, params object[] args) { this.Trace_o(message, args); return returnVal; }




        public T ForwardLogs<T>(T logEvtSrc) where T : ILogSource
        {
            if (logEvtSrc == null) return logEvtSrc;
            if (logEvtSrc.IsLogForwarded) return logEvtSrc;

            logEvtSrc.LogAdded += (s, e) =>
            {
                try
                    { _logAdded?.Invoke(s, e); }
                catch (Exception ex)
                    { LogError("_logAdded.Invoke", ex); }
            };

            logEvtSrc.IsLogForwarded = true;

            return logEvtSrc;
        }


        protected bool LogError(string callerName, Exception ex, bool withTypeNames =false, bool withShortStackTrace = true)
            => Error_n($"Error on {callerName}()", ex.Details(withTypeNames, withShortStackTrace));


        private bool Arg(L4j level, ShowLogAs showAs, string title, string message)
        {
            if (level == L4j.Off) return false;

            var e = new LogEventArg
            {
                Level = level,
                ShowAs = showAs,
                Title = title,
                Message = message
            };

            try
            {
                _logAdded?.Invoke(this, e);
            }
            catch { }

            return e.Level.Polarity();
        }

        public bool Normal_(L4j level, object title, object message, params object[] args) { return Arg(level, ShowLogAs.Normal, title.ToString(), message.ToString().f(args)); }
        public bool Intro_(L4j level, object title, params object[] args) { return Arg(level, ShowLogAs.Intro, title.ToString().f(args), ""); }
        public bool Outro_(L4j level, object message, params object[] args) { return Arg(level, ShowLogAs.Outro, "", message.ToString().f(args)); }



        /*
         *		Uses this.DefaultLevel
         * 
         */
        public bool Normal_(string title, object message, params object[] args) { return Normal_(this.DefaultLevel, title, message, args); }
        public bool Intro_(string title, params object[] args) { return Intro_(this.DefaultLevel, title, args); }
        public bool Outro_(string message, params object[] args) { return Outro_(this.DefaultLevel, message, args); }





    }
}
