using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ErrH.Tools.Loggers
{
    public interface ILogSource
    {
        L4j   DefaultLevel    { get; set; }
        bool  IsLogForwarded  { get; set; }

        event EventHandler<LogEventArg> LogAdded;


        bool Fatal_n(string title, object message, params object[] args);
        bool Fatal_h(string title, object subTitle, params object[] args);
        bool Fatal_i(string title, params object[] args);
        bool Fatal_o(object message, params object[] args);

        T Error_<T>(T returnValue, object title, object message, params object[] args);
        bool Error_n(string title, object message, params object[] args);
        bool Error_h(string title, object subTitle, params object[] args);
        bool Error_i(string title, params object[] args);
        bool Error_o(object message, params object[] args);
        T Error_o_<T>(T returnVal, object message, params object[] args);

        T Warn_<T>(T returnValue, object title, object message, params object[] args);
        bool Warn_n(string title, object message, params object[] args);
        bool Warn_h(string title, object subTitle, params object[] args);
        bool Warn_i(string title, params object[] args);
        bool Warn_o(object message, params object[] args);
        T Warn_o_<T>(T returnVal, object message, params object[] args);

        T Info_<T>(T returnValue, object title, object message, params object[] args);
        bool Info_n(string title, object message, params object[] args);
        bool Info_h(string title, object subTitle, params object[] args);
        bool Info_i(string title, params object[] args);
        bool Info_o(object message, params object[] args);

        T Debug_<T>(T returnValue, object title, object message, params object[] args);
        bool Debug_n(string title, object message, params object[] args);
        bool Debug_h(string title, object subTitle, params object[] args);
        bool Debug_i(string title, params object[] args);
        bool Debug_o(object message, params object[] args);

        T Trace_<T>(T returnValue, object title, object message, params object[] args);
        bool Trace_n(string title, object message, params object[] args);
        bool Trace_h(string title, object subTitle, params object[] args);
        bool Trace_i(string title, params object[] args);
        bool Trace_o(object message, params object[] args);
        T Trace_o_<T>(T returnVal, object message, params object[] args);

        T ForwardLogs<T>(T logEvtSource) where T : ILogSource;



        //HelloGoodbyePattern Tr { get; }

        bool Normal_(string title, object message, params object[] args);
        bool Intro_(string title, params object[] args);
        bool Outro_(string message, params object[] args);


        bool Normal_(L4j level, object title, object message, params object[] args);
        bool Intro_(L4j level, object title, params object[] args);
        bool Outro_(L4j level, object message, params object[] args);
    }
}
