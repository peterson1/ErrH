using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ErrH.RestClient.PCL45.Extensions;
using Newtonsoft.Json;

namespace ErrH.RestClient.PCL45.Readers
{
    public abstract class BasicAuthReaderBulkCacheBase : BasicAuthReader
    {
        private static string _cacheDir;
        private static string _dateFile;


        public bool       CacheEnabled  { get; set; }
        public DateTime   LastUpdate    { get; private set; }


        protected abstract IEnumerable<string> DirectoryGetFiles(string foldrPath);
        protected abstract string   GetFolderPathLocalApplicationData();
        protected abstract string   PathCombine               (string pathPart1, string pathPart2);
        protected abstract void     DirectoryCreateDirectory  (string folderPath);
        protected abstract void     FileDelete                (string filePath);
        protected abstract bool     FileExists                (string filePath);
        protected abstract string   FileReadAllText           (string filePath);
        protected abstract void     FileWriteAllText          (string filePath, string json);



        public BasicAuthReaderBulkCacheBase(string baseUrl, string userName, string password, bool useCache = true) 
            : base(baseUrl, userName, password)
        {
            CacheEnabled = useCache;

            if (_cacheDir.IsBlank())
            {
                _cacheDir = GetCacheDir();
                _dateFile = PathCombine(_cacheDir, "last-node-update.txt");
            }
        }



        public class LastNodeUpdate
        {
            public DateTime node_changed { get; set; }
        }


        public void ClearCache()
        {
            foreach (var file in DirectoryGetFiles(_cacheDir))
                FileDelete(file);
        }



        public async Task EnsureFreshCache(string lastUpd8ViewsURL)
        {
    //var tmp = @"C:\Users\Pete\Desktop\trace\";
    //FileWriteAllText(tmp + "00_url.txt", _baseURL + lastUpd8ViewsURL);

            var orig = CacheEnabled;
            CacheEnabled = false;
            var sevrDTO = await base.List<LastNodeUpdate>(lastUpd8ViewsURL).ConfigureAwait(false);
            CacheEnabled = orig;

    //FileWriteAllText(tmp + "01_json.txt", JsonConvert.SerializeObject(sevrDTO, Formatting.Indented));

            LastUpdate = sevrDTO[0].node_changed;

    //FileWriteAllText(tmp + "02_LastUpdate.txt", LastUpdate.ToString("yyyy-MM-dd HH:mm:ss"));

            var cacheD8 = FileExists(_dateFile) ? DateTime.Parse(FileReadAllText(_dateFile))
                                                 : (DateTime?)null;

    //FileWriteAllText(tmp + "03_cacheD8.txt", cacheD8?.ToString("yyyy-MM-dd HH:mm:ss") ?? "null");

            if (LastUpdate == cacheD8) return;
            //_logr.Trace("cached dates diff'd: [svr: {0}] -vs- [loc: {1}]", LastUpdate, cacheD8);

    //FileWriteAllText(tmp + "04_clearingCache__.txt", "...");

            ClearCache();

    //FileWriteAllText(tmp + "05_writingDateFile__.txt", "...");

            FileWriteAllText(_dateFile, LastUpdate.ToString("yyyy-MM-dd HH:mm:ss"));
        }


        protected override async Task<T> Get<T>(string resource)
        {
            if (!CacheEnabled) return await base.Get<T>(resource).ConfigureAwait(false);

            var cacheF = FindCachePathFor(resource);
            if (FileExists(cacheF))
            {
                var str = FileReadAllText(cacheF);
                return JsonConvert.DeserializeObject<T>(str);
            }

            var obj = await base.Get<T>(resource).ConfigureAwait(false);
            var json = JsonConvert.SerializeObject(obj, Formatting.None);
            FileWriteAllText(cacheF, json);
            return obj;
        }


        private string FindCachePathFor(string resource)
        {
            var keys = $"{_baseURL}{_userName}{resource}{LastUpdate}";
            var fName = keys.SHA1() + ".json";
            return PathCombine(_cacheDir, fName);
        }


        private string GetCacheDir()
        {
            var locApp = GetFolderPathLocalApplicationData();
            var path = PathCombine(locApp, GetType().Name);
            DirectoryCreateDirectory(path);
            return path;
        }

    }
}
