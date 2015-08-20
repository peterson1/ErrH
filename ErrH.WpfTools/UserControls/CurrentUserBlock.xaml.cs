using System.Windows.Controls;
using System.Windows.Media;

namespace ErrH.WpfTools.UserControls
{
    /// <summary>
    /// Interaction logic for CurrentUserBlock.xaml
    /// </summary>
    public partial class CurrentUserBlock : UserControl
    {
        private string _username;


        public CurrentUserBlock()
        {
            InitializeComponent();
        }

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                _block.Foreground = Brushes.SeaGreen;
                _block.Text = $"Hi {_username}.";
                this.UpdateLayout();
            }
        }
    }
}
