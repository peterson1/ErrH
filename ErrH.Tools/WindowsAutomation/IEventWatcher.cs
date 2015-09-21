using System;
using ErrH.Tools.ScalarEventArgs;

namespace ErrH.Tools.WindowsAutomation
{
    public interface IEventWatcher
    {
        event EventHandler<EArg<string>> ProcessStarted;

        void RaiseProcessStarted(string processName);

        void WatchFor(string processName,
                      string eventClassName = "__InstanceCreationEvent",
                      string instanceTyp = "Win32_Process",
                      int pollIntervalSeconds = 1);
    }
}
