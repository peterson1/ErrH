using System.Collections.Generic;
using System.Reflection;
using ErrH.Tools.Extensions;

namespace ErrH.WpfTools.Themes
{
    public abstract class WpfThemeBase : IWpfTheme
    {
        //public string ProjectName  => "ErrH.WpfTools";
        public string ProjectName =>
            Assembly.GetCallingAssembly().GetName().Name;

        //public string ThemesFolder => "Themes";
        public string ThemesFolder =>
            GetType().Namespace.Between(ProjectName + ".", ".");


        public string SubFolder =>
            GetType().Namespace.TextAfter(ThemesFolder + ".");


        public abstract List<string> ResourceFilenames { get; }
    }
}
