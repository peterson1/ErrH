using ErrH.Tools.Loggers;
using ErrH.Tools.MvvmPattern;

namespace ErrH.WpfTools.ViewModels
{
    //later: deprecate this
    public class LogScrollerVM : WorkspaceVmBase
    {
        public ILogSource LogSource { get; }


        //later: add logging to file
        public LogScrollerVM(ILogSource logSource)
        {
            DisplayName = "Event Logs";
            LogSource = logSource;
        }
    }
}
