using System.Windows.Forms;

namespace ErrH.WinTools.DataGridViewTools
{
    public struct Width
    {
        public static int Fill = -2;
        public static int Content = -1;
    }

    public struct ColWidth
    {
        public static DataGridViewAutoSizeColumnMode Fill = DataGridViewAutoSizeColumnMode.Fill;
        public static DataGridViewAutoSizeColumnMode Content = DataGridViewAutoSizeColumnMode.DisplayedCells;
        public static DataGridViewAutoSizeColumnMode None = DataGridViewAutoSizeColumnMode.None;
    }

    public struct ColsWidth
    {
        public static DataGridViewAutoSizeColumnsMode Fill = DataGridViewAutoSizeColumnsMode.Fill;
        public static DataGridViewAutoSizeColumnsMode Content = DataGridViewAutoSizeColumnsMode.DisplayedCells;
        public static DataGridViewAutoSizeColumnsMode None = DataGridViewAutoSizeColumnsMode.None;
    }


    public struct AlignMid
    {
        public static DataGridViewContentAlignment Left = DataGridViewContentAlignment.MiddleLeft;
        public static DataGridViewContentAlignment Right = DataGridViewContentAlignment.MiddleRight;
        public static DataGridViewContentAlignment Center = DataGridViewContentAlignment.MiddleCenter;
    }
}
