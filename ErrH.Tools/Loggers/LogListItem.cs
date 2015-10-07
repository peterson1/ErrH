using System;

namespace ErrH.Tools.Loggers
{
    public class LogListItem : LogEventArg
    {

        public DateTime  EventDate   { get; set; }
        public object    EventSource { get; set; }



        public LogListItem(object eventSource, LogEventArg eventArg)
        {
            this.EventDate   = DateTime.Now;
            this.EventSource = eventSource;

            base.Level       = eventArg.Level;
            base.Title       = eventArg.Title;
            base.Message     = eventArg.Message;
            base.ShowAs      = eventArg.ShowAs;
        }
    }
}
