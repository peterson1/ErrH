using System.Collections.Generic;

namespace ErrH.WpfTools.Themes
{
    public interface IWpfTheme
    {
        string       ProjectName       { get; }
        string       ThemesFolder      { get; }
        string       SubFolder         { get; }
        List<string> ResourceFilenames { get; }
    }
}
