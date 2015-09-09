using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrH.Tools.Loggers;

namespace ErrH.WpfTools.ViewModels
{
    public class LogScrollerVM : ViewModelBase
    {
        public ILogSource LogSource { get; }


        public LogScrollerVM(ILogSource logSource)
        {
            DisplayName = "Event Logs";
            LogSource = logSource;
        }
    }
}
