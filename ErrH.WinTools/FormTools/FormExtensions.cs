using System.Windows.Forms;

namespace ErrH.WinTools.FormTools
{
    public static class FormExtensions
    {

        public static void Minimize(this Form form)
        {
            form.WindowState = FormWindowState.Minimized;
        }

        public static void Maximize(this Form form)
        {
            form.WindowState = FormWindowState.Maximized;
        }

        public static void Restore(this Form form)
        {
            form.WindowState = FormWindowState.Normal;
        }

    }
}
