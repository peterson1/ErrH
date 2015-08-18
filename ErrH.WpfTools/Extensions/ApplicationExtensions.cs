using System;
using System.Windows;
using ErrH.WpfTools.Themes;

namespace ErrH.WpfTools.Extensions
{
    public static class ApplicationExtensions
    {

        /// <summary>
        /// Adds resource to app's dictionary.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="dictionaryXamlUri"></param>
        /// <param name="uriKind"></param>
        public static void AddResource(this Application app, 
            string dictionaryXamlUri, UriKind uriKind)
        {
            var dict = new ResourceDictionary();
            dict.Source = new Uri(dictionaryXamlUri, uriKind);
            app.Resources.MergedDictionaries.Add(dict);
        }



        public static void UseTheme<T>(this Application app) 
            where T : IWpfTheme, new()
        {
            var thme = new T();
            foreach (var xaml in thme.ResourceFilenames)
            {
                //hack: hard-coded namespace
                var uri = $"/ErrH.WpfTools;component/Themes/{thme.SubFolder}/{xaml}";
                app.AddResource(uri, UriKind.Relative);
            }
        }
    }
}
