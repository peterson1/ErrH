using System.Windows.Controls;
using System.Windows.Input;
using AutoDependencyPropertyMarker;
using FontAwesome.WPF;

namespace ErrH.WpfTools.UserControls
{
    [AutoDependencyProperty]
    public partial class ErrButton : UserControl
    {

        public string           Text    { get; set; }
        public FontAwesomeIcon  Icon    { get; set; }
        public ICommand         Command { get; set; }

        public ErrButton()
        {
            //var btn = new Button();
            //btn.com
            //var icon = ImageAwesome.CreateImageSource(FontAwesomeIcon)

            InitializeComponent();
        }
    }
}
