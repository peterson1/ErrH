using ErrH.Tools.DataAttributes;

namespace ErrH.BinUpdater.Core.DTOs
{
    public class ConfigFileDto
    {
        public string base_url { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public int app_nid { get; set; }
        public bool valid_ssl { get; set; }

        [Int(Min = 1)]
        public int mins_interval { get; set; }
    }
}
