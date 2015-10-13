//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Management;
//using ErrH.Tools.Extensions;
//using ErrH.Tools.Loggers;
//using ErrH.Tools.ScalarEventArgs;
//using ErrH.Tools.WindowsAutomation;

//namespace ErrH.WinTools.ProcessTools
//{
//    public class EventWatcherShim : LogSourceBase, IEventWatcher
//    {
//        public event EventHandler<EArg<string>> ProcessStarted;
//        public event EventHandler<EArg<int>>    ProcessExited;


//        private List<ManagementEventWatcher> _watchers = new List<ManagementEventWatcher>();


//        public void WatchFor(string processName, string eventClassName, string instanceTyp, int pollIntervalSeconds)
//        {

//            var condition = $"TargetInstance isa \"{instanceTyp}\""
//                          + $" and TargetInstance.Name = '{processName}'";

//            var qry = new WqlEventQuery(eventClassName,
//                                        new TimeSpan(0, 0, pollIntervalSeconds), 
//                                        condition);
//            var watchr = new ManagementEventWatcher(qry);

//            watchr.EventArrived += (s, e) =>
//            {
//                try { OnEventArrived(processName); }
//                catch (Exception ex) {
//                    Error_n($"Error occured OnEventArrived({processName})", ex.Details(true, false)); }
//            };


//            Debug_n($"Watching for process to start...", $"process name:  “{processName}”");
//            watchr.Start();

//            _watchers.Add(watchr);
//        }

//        private void OnEventArrived(string processName)
//        {
//            Trace_n($"Awaited process found.", $"process name:  “{processName}”");
//            RaiseProcessStarted(processName);
//            var trimmd = processName.TextBefore(".");
//            var proc = Process.GetProcessesByName(trimmd).FirstOrDefault();
//            if (proc == null)
//            {
//                Warn_n("Process.GetProcessesByName() returned NULL", $"Process name not found: {trimmd}");
//                return;
//            }
//            proc.EnableRaisingEvents = true;
//            proc.Exited += (sender, args) =>
//            {
//                try { OnProcessExit(sender); }
//                catch (Exception ex) {
//                    Error_n($"Error occured OnProcessExit({processName})", ex.Details(true, false)); }
//            };
//        }

//        private void OnProcessExit(object sender)
//        {
//            var retCode = sender.As<Process>().ExitCode;
//            Trace_n($"Awaited process exited.", $"exit code :  ‹{retCode}›");
//            RaiseProcessExited(retCode);
//        }

//        public void StartAndWatch(string exeFilePath)
//        {
//            var proc = new Process();
//            proc.StartInfo = new ProcessStartInfo(exeFilePath);
//            proc.EnableRaisingEvents = true;
//            proc.Exited += (sender, args) =>
//            {
//                var retCode = sender.As<Process>().ExitCode;
//                Trace_n($"Awaited process exited.", $"exit code :  ‹{retCode}›");
//                RaiseProcessExited(retCode);
//            };
//            proc.Start();
//        }



//        public void RaiseProcessStarted(string processName)
//            => ProcessStarted?.Invoke(this, new EArg<string> { Value = processName });

//        public void RaiseProcessExited(int exitCode)
//            => ProcessExited?.Invoke(this, new EArg<int> { Value = exitCode });



//        public void Dispose()
//        {
//            foreach (var watchr in _watchers)
//            {
//                watchr.Stop();
//                watchr.Dispose();
//            }
//            _watchers.Clear();
//        }
//    }
//}
