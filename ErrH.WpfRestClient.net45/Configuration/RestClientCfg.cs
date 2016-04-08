using ErrH.Wpf.net45.BinUpdater;
using ErrH.Wpf.net45.Configuration;
using Newtonsoft.Json;

namespace ErrH.WpfRestClient.net45.Configuration
{
    public class RestClientCfg : SettingsFileBase
    {
        public string    Username       { get; set; }
        public string    Password       { get; set; }
        public string    BaseURL        { get; set; }
        public int       UserID         { get; set; }
        public string    ServerThumb    { get; set; }

        public bool      StartMaximized { get; set; }

        public BinUpdaterKey BinUpdater { get; set; }



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

            return new RestClientCfg
            {
                Username   = "username goes here",
                Password   = "password goes here",
                BaseURL    = "https://url.goes.here/api/",
                UserID     = 1234,
                ServerThumb = "X509_server_thumb_goes_here",
                BinUpdater = updatr,
            };
        }
    }
}
