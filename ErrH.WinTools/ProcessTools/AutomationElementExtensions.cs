using System.Windows.Automation;

namespace ErrH.WinTools.ProcessTools
{
    public static class AutomationElementExtensions
    {

        /// <summary>
        /// Gets text of MessageBox.
        /// </summary>
        /// <param name="elm"></param>
        /// <returns></returns>
        public static string DialogText(this AutomationElement elm)
        {
            AutomationElement dlgText = null;
            try
            {
                dlgText = elm.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Text));
            }
            catch { }
            return dlgText?.Current.Name ?? "";
        }
    }
}
