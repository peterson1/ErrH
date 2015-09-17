using System.Windows.Controls;
using AutoDependencyPropertyMarker;
using FontAwesome.WPF;

namespace ErrH.WpfTools.UserControls
{
    [AutoDependencyProperty]
    public partial class TextAndIcon : UserControl
    {

        public string           Text    { get; set; }
        public FontAwesomeIcon  Icon    { get; set; }



        public TextAndIcon()
        {
            InitializeComponent();
        }
    }
}
