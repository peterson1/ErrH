using System;
using System.IO;
using System.Threading.Tasks;
using ErrH.Tools.Extensions;
using Newtonsoft.Json;
using NLog;
using static System.Environment;

namespace ErrH.WpfRestClient.net45.DataAccess
{
    public class BulkCacheD7Client : D7Client
    {
        private const  string  _lastUpd8ViewsURL = "/views/last_node_update?display_id=page";
        private static Logger  _logr             = LogManager.GetCurrentClassLogger();
        private static string  _cacheDir         = GetCacheDir();
        private static string  _dateFile         = Path.Combine(_cacheDir, "last-node-update.txt");

        public bool      CacheEnabled { get; set; }
        public DateTime  LastUpdate   { get; private set; }


        public BulkCacheD7Client(string baseUrl, string userName, string password, int userID = 0, bool useCache = true) : base(baseUrl, userName, password, userID)
        {
            CacheEnabled = useCache;
        }


        public class LastNodeUpdate
        {
            public DateTime node_changed { get; set; }
        }


        public void ClearCache()
        {
            foreach (var file in Directory.GetFiles(_cacheDir))
                File.Delete(file);
        }


        public async Task EnsureFreshCache()
        {
            var sevrDTO = await List<LastNodeUpdate>(_lastUpd8ViewsURL);
            LastUpdate  = sevrDTO[0].node_changed;

            var cacheD8 = File.Exists(_dateFile) ? DateTime.Parse(File.ReadAllText(_dateFile))
                                                 : (DateTime?)null;
            if (LastUpdate == cacheD8) return;
            _logr.Trace("cached dates diff'd: [svr: {0}] -vs- [loc: {1}]", LastUpdate, cacheD8);
            ClearCache();

            File.WriteAllText(_dateFile, LastUpdate.ToString("yyyy-MM-dd hh:mm:ss"));
        }


        protected override async Task<T> Get<T>(string resource)
        {
            if (!CacheEnabled) return await base.Get<T>(resource);

            var cacheF = FindCachePathFor(resource);
            if (File.Exists(cacheF))
            {
                var str = File.ReadAllText(cacheF);
                return JsonConvert.DeserializeObject<T>(str);
            }

            var obj  = await base.Get<T>(resource);
            var json = JsonConvert.SerializeObject(obj, Formatting.None);
            File.WriteAllText(cacheF, json);
            return obj;
        }


        private string FindCachePathFor(string resource)
        {
            var keys  = $"{_baseURL}{_userName}{resource}{LastUpdate}";
            var fName = keys.SHA1() + ".json";
            return Path.Combine(_cacheDir, fName);
        }


        private static string GetCacheDir()
        {
            var locApp = Environment.GetFolderPath(SpecialFolder.LocalApplicationData);
            var path   = Path.Combine(locApp, typeof(BulkCacheD7Client).Name);
            Directory.CreateDirectory(path);
            return path;
        }
    }
}
