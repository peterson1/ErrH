using System.Collections.Generic;
using ErrH.Wpf.net45.BinUpdater;
using ErrH.Wpf.net45.Configuration;
using Newtonsoft.Json;

namespace ErrH.D7Poster.WPF.Configuration
{
    public class SettingsCfg : SettingsFileBase
    {
        public string        Username   { get; set; }
        public string        Password   { get; set; }
        public string        BaseURL    { get; set; }
        public int           UserID     { get; set; }
        public List<Target>  Targets    { get; set; }
        public BinUpdaterKey BinUpdater { get; set; }


        public class Target
        {
            public string  Title   { get; set; }
            public string  Source  { get; set; }
            public string  Filter  { get; set; }
            public string  Archive { get; set; }
        }


        protected override string SerializeObj(SettingsFileBase obj)
            => JsonConvert.SerializeObject(obj, Formatting.Indented);


        protected override T DeserializeStr<T>(string str)
            => JsonConvert.DeserializeObject<T>(str);


        protected override SettingsFileBase CreatePlaceholderObj()
        {
            var updatr = new BinUpdaterKey
            {
                Username = "username goes here",
                Password = "password goes here",
                BaseURL  = "https://url.goes.here/api/",
                AppNid   = 1234,
                EveryMin = 2,
            };

            var target = new Target
            {
                Title   = "target title goes here",
                Source  = @"C:\some-directory\somewhere\logs",
                Archive = @"C:\some-directory\somewhere\logs\archive",
                Filter  = "*.log"
            };

            return new SettingsCfg
            {
                Username   = "username goes here",
                Password   = "password goes here",
                BaseURL    = "https://url.goes.here/api/",
                UserID     = 1234,
                Targets    = new List<Target> { target },
                BinUpdater = updatr,
            };
        }

    }
}
