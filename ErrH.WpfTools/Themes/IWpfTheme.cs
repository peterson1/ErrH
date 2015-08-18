using System.Collections.Generic;

namespace ErrH.WpfTools.Themes
{
    public interface IWpfTheme
    {
        List<string> ResourceFilenames { get; }
        string       SubFolder         { get; }
    }
}
