using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace ErrH.Wpf.net45.Extensions
{
    public static class WindowExtensions
    {
        public static void FitToScreenHeight(this Window win, int displayIndex = -1)
        {
            var area = Display(displayIndex).WorkingArea;
            win.Top = area.Top;
            win.Height = area.Height;
        }


        public static void CenterHorizontally(this Window win, int displayIndex = -1)
        {
            var area = Display(displayIndex).WorkingArea;
            win.Left = area.X + ((area.Width / 2) - (win.Width / 2));
        }


        private static Screen Display(int displayIndex)
        {
            var all = Screen.AllScreens;
            return all[displayIndex == -1 ? all.Length - 1 : displayIndex];
        }


        public static void Send(this Window win, Key key)
        {
            if (Keyboard.PrimaryDevice != null)
            {
                if (Keyboard.PrimaryDevice.ActiveSource != null)
                {
                    var e = new System.Windows.Input.KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, key)
                    {
                        RoutedEvent = Keyboard.KeyDownEvent
                    };
                    InputManager.Current.ProcessInput(e);

                    // Note: Based on your requirements you may also need to fire events for:
                    // RoutedEvent = Keyboard.PreviewKeyDownEvent
                    // RoutedEvent = Keyboard.KeyUpEvent
                    // RoutedEvent = Keyboard.PreviewKeyUpEvent
                }
            }
        }

    }
}
