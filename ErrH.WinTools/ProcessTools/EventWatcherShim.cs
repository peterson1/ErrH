using System;
using System.Collections.Generic;
using System.Management;
using ErrH.Tools.ScalarEventArgs;
using ErrH.Tools.WindowsAutomation;

namespace ErrH.WinTools.ProcessTools
{
    public class EventWatcherShim : IEventWatcher, IDisposable
    {
        public event EventHandler<EArg<string>> ProcessStarted;


        private List<ManagementEventWatcher> _watchers = new List<ManagementEventWatcher>();


        public void WatchFor(string processName, string eventClassName, string instanceTyp, int pollIntervalSeconds)
        {
            var condition = $"TargetInstance isa \"{instanceTyp}\"" 
                          + $" and TargetInstance.Name = '{processName}'";

            var query = new WqlEventQuery(eventClassName, 
                            new TimeSpan(0, 0, pollIntervalSeconds), condition);

            var watchr = new ManagementEventWatcher(query);
            watchr.EventArrived += (s, e) 
                => { RaiseProcessStarted(processName); };
            watchr.Start();

            _watchers.Add(watchr);
        }


        public void RaiseProcessStarted(string processName)
            => ProcessStarted?.Invoke(this, new EArg<string> { Value = processName });



        public void Dispose()
        {
            foreach (var watchr in _watchers)
            {
                watchr.Stop();
                watchr.Dispose();
            }
            _watchers.Clear();
        }
    }
}
