using System.Windows.Controls;
using System.Windows.Input;
using ErrH.WpfTools.Extensions;

namespace ErrH.WpfTools.UserControls
{
    /// <summary>
    /// Interaction logic for UserSessionMenu.xaml
    /// </summary>
    public partial class UserSessionMenu : UserControl
    {
        public UserSessionMenu()
        {
            InitializeComponent();

            Loaded += (s, e) =>
            {
                inputsPanel.IsVisibleChanged += (t, f) 
                    => Keyboard.ClearFocus();

                tbxPwd.DoLogin(btnLogin.Command, lblPwd);
            };

        }
    }
}
