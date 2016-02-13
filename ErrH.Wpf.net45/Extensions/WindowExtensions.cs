using System.Windows;

namespace ErrH.Wpf.net45.Extensions
{
    public static class WindowExtensions
    {
        public static void CenterOnScreen(this Window win)
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth  = win.Width;
            double windowHeight = win.Height;
            win.Left = (screenWidth / 2) - (windowWidth / 2);
            win.Top = (screenHeight / 2) - (windowHeight / 2);
        }
    }
}
