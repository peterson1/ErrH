using System.Collections.Generic;

namespace ErrH.WpfTools.Themes.ErrHBaseTheme
{
    public class ErrHBase : IWpfTheme
    {
        public List<string> ResourceFilenames
            => new List<string> {
                "Constants.xaml",
                "WindowStyles.xaml",
                "ContentTabs.xaml",
                "ContentList.xaml",
                "ResizableExpanders.xaml",
                "MainListBox.xaml",
                "TabItemStyles.xaml",
                "TabControlStyles.xaml",
                "DataGridStyles.xaml",
                "BusyIndicatorStyles.xaml"
            };

        public string SubFolder => "ErrHBaseTheme";
    }
}
