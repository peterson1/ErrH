using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.Windows.Automation;
using System.Windows.Forms;
using ErrH.Tools.Extensions;
using ErrH.Tools.Loggers;
using ErrH.Tools.ScalarEventArgs;
using ErrH.Tools.WindowsAutomation;
using ErrH.Tools.WindowsAutomation.ItemShims;

namespace ErrH.WinTools.ProcessTools
{
    public class InstanceCreationWatcher : LogSourceBase, IUiEventWatcher
    {
        private       EventHandler _instanceCreated;
        public  event EventHandler  InstanceCreated
        {
            add    { _instanceCreated -= value; _instanceCreated += value; }
            remove { _instanceCreated -= value; }
        }

        private      EventHandler<EArg<MessageBoxShim>> _windowOpened;
        public event EventHandler<EArg<MessageBoxShim>>  WindowOpened
        {
            add    { _windowOpened -= value; _windowOpened += value; }
            remove { _windowOpened -= value; }
        }


        private ManagementEventWatcher _watchr;
        private string                 _procName;
        private List<int>              _procIDs = new List<int>();
        private List<AutomationEvtH>   _handlrs = new List<AutomationEvtH>();



        private ManagementEventWatcher CreateWatcher(string processName,
                                                     string eventClassName = "__InstanceCreationEvent",
                                                     string instanceTyp = "Win32_Process",
                                                     int pollIntervalSeconds = 2)
        {
            var condition = $"TargetInstance isa '{instanceTyp}'" 
                          + $" and TargetInstance.Name = '{processName}'";

            var qry = new WqlEventQuery(eventClassName, 
                new TimeSpan(0, 0, pollIntervalSeconds), condition);

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


            // if already running, add handlers
            var procs = GetTargetProcesses();
            if (procs.Count > 0)
            {
                Trace_n("Instance already exists", "");
                AddEventHandlers(procs);
                _instanceCreated?.Invoke(this, EventArgs.Empty);
            }



            try
            {
                _watchr = CreateWatcher(_procName);

                _watchr.EventArrived += 
                    new EventArrivedEventHandler(OnInstanceCreated);

                _watchr.Start();

                Info_n($"Waiting for Instance Creation...", $"TargetInstance.Name: “{_procName}”");
            }
            catch (Exception ex)
                { LogError("InstanceCreationWatcher.WatchFor", ex); }
        }



        private void OnInstanceCreated(object sender, EventArrivedEventArgs e)
        {
            Trace_n("InstanceCreated event triggered", "");
            _instanceCreated?.Invoke(sender, e);

            var procs = GetTargetProcesses();
            if (procs.Count == 0) return;

            AddEventHandlers(procs);
        }


        private void AddEventHandlers(List<Process> procs)
        {
            foreach (var proc in procs)
            {
                var evtH = new AutomationEvtH
                {
                    Element = AutomationElement.RootElement,
                    EventID = WindowPattern.WindowOpenedEvent,
                    OnAutomationEvent = new AutomationEventHandler(OnUIAutomationEvent),
                };
                _handlrs.Add(evtH);

                Automation.AddAutomationEventHandler(evtH.EventID,
                    evtH.Element, TreeScope.Subtree, evtH.OnAutomationEvent);
            }
        }


        private void OnAutomationFocusChanged(object s, AutomationFocusChangedEventArgs e)
        {
            MessageBox.Show($"e.ObjectId: {e.ObjectId}"
                     + L.f + $"e.ChildId: {e.ChildId}"
                     + L.f + $"e.EventId: {e.EventId}");
        }


        private void OnStructureChanged(object sender, StructureChangedEventArgs e)
        {
            MessageBox.Show($"e.StructureChangeType: {e.StructureChangeType}"
                                + L.f + $"e.EventId: {e.EventId}");
        }


        private void OnUIAutomationEvent(object sender, AutomationEventArgs e)
        {
            //MessageBox.Show("OnUIAutomationEvent");
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

                var procID = sourceElement.Current.ProcessId;
                if (!_procIDs.Contains(procID)) return;

                string nme = "";
                try { nme = sourceElement.Current.Name; }
                catch (Exception ex)
                    { LogError("sourceElement.Current.Name", ex); }



                //var strText = sourceElement.DialogText();
                //MessageBox.Show("MessageBox says:" + L.F + strText);
                //MessageBox.Show($"procID = {procID}" + L.f + $"“{nme}”");

                var shim = new MessageBoxShim
                {
                    Title = sourceElement.Current.Name,
                    Text = sourceElement.DialogText()
                };

                _windowOpened?.Invoke(sender, new EArg<MessageBoxShim> { Value = shim });
            }
            else
            {
                //Trace_n("Unexpected event occured.", $"[{e.EventId}]");
            }
        }




        private List<Process> GetTargetProcesses()
        {
            var list = new List<Process>();
            Process[] procs;

            var nme = _procName.TextBefore(".exe");

            try { procs = Process.GetProcessesByName(nme); }
            catch (Exception ex)
                { return Warn_(list, "Error in Process.GetProcessesByName()", $"Process name: “{nme}”" + L.F + ex.Details(false, false)); }

            Trace_n("Process.GetProcessesByName() finished.", $"found {procs.Length.x("process")}");

            foreach (var p in procs)
            {
                list.Add(p);
                _procIDs.Add(p.Id);
                //MessageBox.Show($"matching procID: {p.Id}");
            }

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
                try
                {
                    if (h.OnAutomationEvent != null)
                        Automation.RemoveAutomationEventHandler(h.EventID, h.Element, h.OnAutomationEvent);

                    if (h.OnFocusChanged != null)
                        Automation.RemoveAutomationFocusChangedEventHandler(h.OnFocusChanged);

                    if (h.OnStructureChanged != null)
                        Automation.RemoveStructureChangedEventHandler(h.Element, h.OnStructureChanged);
                }
                catch { }
            }
        }
    }
}
