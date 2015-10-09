using System.Windows.Automation;

namespace ErrH.WinTools.ProcessTools
{
    public class AutomationEvtH
    {
        public AutomationEvent    EventID { get; set; }
        public AutomationElement  Element { get; set; }

        public AutomationEventHandler             OnAutomationEvent  { get; set; }
        public StructureChangedEventHandler       OnStructureChanged { get; set; }
        public AutomationFocusChangedEventHandler OnFocusChanged     { get; set; }
    }
}
