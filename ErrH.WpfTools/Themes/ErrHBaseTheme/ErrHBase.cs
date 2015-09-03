using System.Collections.Generic;

namespace ErrH.WpfTools.Themes.ErrHBaseTheme
{
    public class ErrHBase : WpfThemeBase
    {
        public override List<string> ResourceFilenames
            => new List<string> {
                "Constants.xaml",
                "WindowStyles.xaml",
                "TextBlockStyles.xaml",
                "ContentList.xaml",
                "ResizableExpanders.xaml",
                "MainListBox.xaml",
                "TabItemStyles.xaml",
                "TabControlStyles.xaml",
                "DataGridStyles.xaml",
                "BusyIndicatorStyles.xaml",
                "DataTemplates.xaml"
            };
    }
}
