using System.Collections.Generic;

namespace ErrH.WpfTools.Themes.BasicPlainTheme
{
    public class BasicPlain : WpfThemeBase
    {
        public override List<string> ResourceFilenames
            => new List<string> {
                "BasicPlain.xaml"
            };
    }
}
