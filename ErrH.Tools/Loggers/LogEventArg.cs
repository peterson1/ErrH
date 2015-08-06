using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ErrH.Tools.Loggers
{
    public class LogEventArg : EventArgs
    {
        public L4j Level { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public ShowLogAs ShowAs { get; set; }
    }
}
