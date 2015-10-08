using System.Windows.Controls;

namespace ErrH.WpfTools.UserControls
{
    public partial class XtkLogScroller : UserControl
    {
        public XtkLogScroller()
        {
            InitializeComponent();

            //todo: uncomment after debugging
            //_rtb.TextChanged += (s, e)
            //    => { _rtb.ScrollToEnd(); };
        }
    }
}
