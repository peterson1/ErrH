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

        public static void CenterVertically(this Window win, int displayIndex = -1)
        {
            var area = Display(displayIndex).WorkingArea;
            win.Top = area.Y + ((area.Height / 2) - (win.Height / 2));
        }


        private static Screen Display(int displayIndex)
        {
            var all = Screen.AllScreens;
            return all[displayIndex == -1 ? all.Length - 1 : displayIndex];
        }

        public static void MakeDraggable(this Window win)
        {
            win.MouseDown -= Window_MouseDown_Handler;
            win.MouseDown += Window_MouseDown_Handler;
        }

        private static void Window_MouseDown_Handler(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;
            var win = sender as Window;
            win?.DragMove();
        }



        //public static void Send(this Window win, Key key)
        //{
        //    win.Send()
        //}

    }
}
