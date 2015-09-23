using System;
using ErrH.Tools.Loggers;
using ErrH.Tools.ScalarEventArgs;

namespace ErrH.Tools.WindowsAutomation
{
    public interface IEventWatcher : ILogSource, IDisposable
    {
        event EventHandler<EArg<string>> ProcessStarted;
        event EventHandler<EArg<int>>    ProcessExited;

        void RaiseProcessStarted(string processName);

        void WatchFor(string processName,
                      string eventClassName = "__InstanceCreationEvent",
                      string instanceTyp = "Win32_Process",
                      int pollIntervalSeconds = 2);

        void StartAndWatch(string exeFilePath);
    }
}
