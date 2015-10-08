using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;
using ErrH.Tools.Extensions;
using ErrH.Tools.Loggers;
using ErrH.Tools.ScalarEventArgs;

namespace ErrH.WinTools.ProcessTools
{
    public class InstanceCreationWatcher : LogSourceBase, IDisposable
    {
        private      EventHandler _instanceCreated;
        public event EventHandler  InstanceCreated
        {
            add    { _instanceCreated -= value; _instanceCreated += value; }
            remove { _instanceCreated -= value; }
        }

        private      EventHandler<EArg<string>> _windowOpened;
        public event EventHandler<EArg<string>>  WindowOpened
        {
            add    { _windowOpened -= value; _windowOpened += value; }
            remove { _windowOpened -= value; }
        }


        private ManagementEventWatcher _watchr;
        private string                 _procName;
        private List<AutomationEvtH>   _handlrs = new List<AutomationEvtH>();



        private ManagementEventWatcher CreateWatcher(string processName,
                                                     string eventClassName = "__InstanceCreationEvent",
                                                     string instanceTyp = "Win32_Process",
                                                     int pollIntervalSeconds = 2)
        {
            string condition = "TargetInstance isa 'Win32_Process' and TargetInstance.Name = 'wwnotray.exe'";

            WqlEventQuery qry = new WqlEventQuery("__InstanceCreationEvent", new TimeSpan(0, 0, 2), condition);

            Trace_n(eventClassName, condition);

            var watchr = new ManagementEventWatcher(qry);

            //watchr.EventArrived += 
            //    new EventArrivedEventHandler(HandleEvent);

            return watchr;
        }



        //private void HandleEvent(object sender, EventArrivedEventArgs e)
        //{
        //    MessageBox.Show("InstanceCreated event triggered");
        //    Trace_n("InstanceCreated event triggered", "");
        //}


        public void WatchFor(string instanceName)
        {
            if (!_procName.IsBlank())
            {
                Warn_n("You can't change the watched target.", $"Currently watching “{_procName}”");
                return;
            }

            _procName = instanceName;

            try {
                _watchr = CreateWatcher(_procName);

                //_watchr.EventArrived += OnInstanceCreated;
                _watchr.EventArrived += 
                    new EventArrivedEventHandler(OnInstanceCreated);

                _watchr.Start();

                Trace_n($"Waiting for Instance Creation Event...", $"TargetInstance.Name: “{_procName}”");
            }
            catch (Exception ex)
                { LogError("InstanceCreationWatcher.WatchFor", ex); }
        }


        private void OnInstanceCreated(object sender, EventArrivedEventArgs e)
        {
            MessageBox.Show("OnInstanceCreated() - 1");

            Trace_n("InstanceCreated event triggered", "");

            MessageBox.Show("OnInstanceCreated() - 2 - after Trace()");

            _instanceCreated?.Invoke(sender, e);

            MessageBox.Show("OnInstanceCreated() - 3 - after Invoke()");

            var procs = GetTargetProcesses();
            if (procs.Count == 0) return;

            foreach (var proc in procs)
            {
                var evtH = new AutomationEvtH
                {
                    Element = AutomationElement.FromHandle(proc.Handle),
                    EventID = WindowPattern.WindowOpenedEvent,
                    Handler = new AutomationEventHandler(OnUIAutomationEvent)
                };
                _handlrs.Add(evtH);
                Automation.AddAutomationEventHandler(evtH.EventID, 
                    evtH.Element, TreeScope.Subtree, evtH.Handler);
            }
        }

        private void OnUIAutomationEvent(object sender, AutomationEventArgs e)
        {
            // Make sure the element still exists. Elements such as tooltips
            // can disappear before the event is processed.
            AutomationElement sourceElement;
            try
            {
                sourceElement = sender as AutomationElement;
            }
            catch (ElementNotAvailableException)
            {
                Trace_n("sourceElement is not available", "");
                return;
            }
            if (e.EventId == WindowPattern.WindowOpenedEvent)
            {
                if (sourceElement == null)
                {
                    Warn_n("sourceElement is NULL", "");
                    return;
                }

                string nme = "";
                try { nme = sourceElement.Current.Name; }
                catch (Exception ex)
                    { LogError("sourceElement.Current.Name", ex); }

                _windowOpened?.Invoke(sender, new EArg<string> { Value = nme });
            }
            else
            {
                Trace_n("Unexpected event occured.", $"[{e.EventId}]");
            }
        }




        private List<Process> GetTargetProcesses()
        {
            var list = new List<Process>();
            Process[] procs;

            try { procs = Process.GetProcessesByName(_procName); }
            catch (Exception ex)
                { return Warn_(list, "Error in Process.GetProcessesByName()", $"Process name: “{_procName}”" + L.F + ex.Details(false, false)); }

            Trace_n("Process.GetProcessesByName() finished.", $"found {procs.Length.x("process")}");

            foreach (var p in procs) list.Add(p);

            return list;
        }



        public void Dispose()
        {
            Trace_n($"Disposing ‹{GetType().Name}›...", "");

            if (_watchr == null) return;
            _watchr.Stop();
            _watchr.Dispose();

            foreach (var h in _handlrs)
            {
                try   { Automation.RemoveAutomationEventHandler(h.EventID, h.Element, h.Handler); }
                catch { }
            }
        }
    }
}
