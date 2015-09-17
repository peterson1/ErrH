using ErrH.Tools.Loggers;
using ErrH.Tools.MvvmPattern;

namespace ErrH.WpfTools.ViewModels
{
    public class LogScrollerVM : ListItemVmBase
    {
        public ILogSource LogSource { get; }


        public LogScrollerVM(ILogSource logSource)
        {
            DisplayName = "Event Logs";
            LogSource = logSource;
        }
    }
}
