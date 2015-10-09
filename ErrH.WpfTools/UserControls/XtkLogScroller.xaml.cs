using System.Windows.Controls;

namespace ErrH.WpfTools.UserControls
{
    public partial class XtkLogScroller : UserControl
    {
        public XtkLogScroller()
        {
            InitializeComponent();

            _rtb.TextChanged += (s, e)
                => { _rtb.ScrollToEnd(); };
        }
    }
}
