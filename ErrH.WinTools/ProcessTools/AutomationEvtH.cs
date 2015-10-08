using System.Windows.Automation;

namespace ErrH.WinTools.ProcessTools
{
    public class AutomationEvtH
    {
        public AutomationEvent        EventID { get; set; }
        public AutomationElement      Element { get; set; }
        public AutomationEventHandler Handler { get; set; }
    }
}
