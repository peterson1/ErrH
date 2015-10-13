using System.Windows.Controls;

namespace ErrH.WpfTools.UserControls
{
    public partial class LogScroller : UserControl
    {

        public LogScroller()
        {
            InitializeComponent();

            _rtb.TextChanged += (s, e)
                => { _rtb.ScrollToEnd(); };
        }
    }
}
