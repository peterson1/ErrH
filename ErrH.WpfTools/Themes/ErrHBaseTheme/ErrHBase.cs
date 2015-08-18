using System.Collections.Generic;

namespace ErrH.WpfTools.Themes.ErrHBaseTheme
{
    public class ErrHBase : IWpfTheme
    {
        public List<string> ResourceFilenames
            => new List<string> {
                "WindowStyles.xaml",
                "ContentTabs.xaml",
                "ContentList.xaml",
                "ResizableExpanders.xaml",
                "MainListBox.xaml",
                "TabItemStyles.xaml",
                "TabControlStyles.xaml",
            };

        public string SubFolder => "ErrHBaseTheme";
    }
}
