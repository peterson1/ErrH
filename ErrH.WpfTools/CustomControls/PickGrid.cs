using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ErrH.WpfTools.CustomControls
{
    public class PickGrid : DataGrid
    {
        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return null;
        }

        static PickGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PickGrid), new FrameworkPropertyMetadata(typeof(PickGrid)));
        }
    }
}
