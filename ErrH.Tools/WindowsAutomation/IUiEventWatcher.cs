using System;
using ErrH.Tools.Loggers;
using ErrH.Tools.ScalarEventArgs;
using ErrH.Tools.WindowsAutomation.ItemShims;

namespace ErrH.Tools.WindowsAutomation
{
    public interface IUiEventWatcher : ILogSource, IDisposable
    {
        event EventHandler                       InstanceCreated;
        event EventHandler<EArg<MessageBoxShim>> WindowOpened;


        void WatchFor(string instanceName);
    }
}
