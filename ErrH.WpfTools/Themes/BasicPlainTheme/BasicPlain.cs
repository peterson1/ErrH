using System.Collections.Generic;

namespace ErrH.WpfTools.Themes.BasicPlainTheme
{
    public class BasicPlain : IWpfTheme
    {
        public List<string> ResourceFilenames
            => new List<string> {
                "BasicPlain.xaml"
            };

        public string SubFolder => "BasicPlainTheme";
    }
}
