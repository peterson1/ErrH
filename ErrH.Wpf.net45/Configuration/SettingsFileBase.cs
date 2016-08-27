using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using ErrH.Tools.Extensions;
using ErrH.Wpf.net45.Extensions;

namespace ErrH.Wpf.net45.Configuration
{
    public abstract class SettingsFileBase
    {
        private static SettingsFileBase _cfg;

        protected virtual string Filename => "settings.cfg";
        protected virtual string Folder   => AppDomain.CurrentDomain.BaseDirectory;

        protected abstract SettingsFileBase CreatePlaceholderObj();
        protected abstract string SerializeObj(SettingsFileBase obj);
        protected abstract T DeserializeStr<T>(string str);


        public static T Load<T>() where T : SettingsFileBase, new()
        {
            if (_cfg != null) return _cfg as T;

            var tmpCfg = new T();
            var str = tmpCfg.ReadSettingsFile();
            if (str.IsBlank()) return null;
            _cfg = tmpCfg.DeserializeStr<T>(str);
            return _cfg as T;
        }


        protected virtual string ReadSettingsFile()
        {
            var path = Path.Combine(Folder, Filename);
            var file = new FileInfo(path);
            if (!file.Exists)
            {
                CreateBlankCfgFile(path);
                file.Hide();
                OpenExplorerWindow(path);
                return "";
            };
            return File.ReadAllText(file.FullName);
        }



        private void CreateBlankCfgFile(string filePath)
        {
            var blank  = CreatePlaceholderObj();
            var serial = SerializeObj(blank);
            File.WriteAllText(filePath, serial);
        }


        private void OpenExplorerWindow(string filePath)
        {
            MessageBox.Show("Settings file not found." + L.F
                         + $"file name :  “{Filename}”" + L.f
                         + $"folder :  {Folder}" + L.F
                         +  "A new file has been created for you in the above folder." + L.f
                         +  "Please edit the file to replace the placeholder values with actual ones.", 
                            "Please create a Settings file.", 
                            MessageBoxButton.OK, 
                            MessageBoxImage.Information);

            var arg = @"/select, " + filePath;
            Process.Start("explorer.exe", arg);
        }
   }
}
