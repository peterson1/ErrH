using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ErrH.Tools.Extensions;
using ErrH.WinTools.Cryptography;

namespace ErrH.WpfTools.Extensions
{
    public static class PasswordBoxExtensions
    {
        public static void DoLogin(this PasswordBox pwdBox, 
            ICommand cmdToRun, FrameworkElement elementToHide)
        {
            pwdBox.PasswordChanged += (s, e) =>
            {
                var consumr = elementToHide.DataContext as ISecureStringConsumer;
                if (consumr != null) consumr.ReceiveKey(pwdBox.SecurePassword);

                elementToHide.Visibility = pwdBox.Password.IsBlank()
                    ? Visibility.Visible : Visibility.Collapsed;
            };

            pwdBox.KeyDown += (s, e) =>
            {
                if (e.Key == Key.Enter)
                    cmdToRun.ExecuteIfItCan(null);
            };
        }
    }
}
