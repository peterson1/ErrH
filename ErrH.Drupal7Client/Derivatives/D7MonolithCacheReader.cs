using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ErrH.RestSharpShim;
using ErrH.Tools.Extensions;
using ErrH.Tools.FileSystemShims;
using ErrH.Tools.RestServiceShim;
using ErrH.Tools.Serialization;

namespace ErrH.Drupal7Client.Derivatives
{
    public class D7MonolithCacheReader : D7BasicCacheReader
    {
        public const string RESOURCE = "last-node-update";
        private string   _changed = null;
        private FileShim _cachedDateFile = null;


        public D7MonolithCacheReader(IFileSystemShim fsShim, ISerializer serializer) : base(fsShim, serializer)
        {
            _cacheSubDir = "Monolithic Cache";
        }



        //protected override bool TryReadCache<T>(string resource, out T result)
        //{
        //    if (_changed.IsBlank())
        //        _changed = GetLastUpdated(resource);

        //    if (_changed.IsBlank())
        //    {
        //        result = default(T);
        //        return false;
        //    }
        //    return base.TryReadCache<T>(resource, out result);
        //}


        //private string GetLastUpdated(string resource)
        //{
        //    if (_dir == null) _dir = GetCacheFolder();
        //    var allF = _dir.Files();
        //    if (allF.Count == 0) return null;

        //    var fName = allF.ToList()
        //                .OrderByDescending(x => x.Name)
        //                .First().Name;

        //    return fName.Substring(0, 16);
        //}


        protected override async Task<T> TryReadCache<T>(string resource)
        {
            var fsJob = base.TryReadCache<T>(resource);
            var d7Job = OtherTask(resource, new CancellationToken());

            try {
                await TaskEx.WhenAny(fsJob, d7Job);
            }
            catch (Exception ex){ LogError("TaskEx.WhenAny(fsJob, d7Job)", ex); }

            return await fsJob;
        }


        protected override async Task<bool> OtherTask(string resource, CancellationToken cancelToken)
        {
            var req = new RequestShim(RESOURCE, RestMethod.Get);
            if (_changed.IsBlank()) _changed = ReadCachedDate();

            List<LastNodeUpdate> res; try
            {
                res = await _client.Send<List<LastNodeUpdate>>(req, cancelToken);
            }
            catch (Exception ex){ return LogError("_client.Send<List<LastNodeUpdate>>", ex); }

            if (res == null)   return Error_n("res == null", "");
            if (res.Count < 1) return Error_n("res.Count < 1", "");

            var d7Changed = res[0].changed;
            if (d7Changed.IsBlank()) return Error_n("d7Changed.IsBlank", "");
            d7Changed = d7Changed.Replace(":", "_")
                                 .Replace(" ", "_");

            if (d7Changed == _changed)
                Trace_n("d7Changed == _changed", d7Changed);
            else
            {
                if (_dir == null) _dir = GetCacheFolder();
                //_dir.Files().ForEach(x => x.Delete());
                await TaskEx.Delay(1);
                if (_dir.Delete())
                    Trace_n($"{d7Changed} vs {_changed}", "Previous cache deleted");

                await TaskEx.Delay(1);
                WriteCachedDate(d7Changed);
                _changed = d7Changed;
            }
            return true;
        }


        private void WriteCachedDate(string d7Changed)
        {
            if (_cachedDateFile == null)
                _cachedDateFile = GetCachedDateFile();

            _cachedDateFile.Write(d7Changed);
        }


        private string ReadCachedDate()
        {
            if (_cachedDateFile == null)
                _cachedDateFile = GetCachedDateFile();

            if (!_cachedDateFile.Found) return null;
            return _cachedDateFile.ReadUTF8;
        }


        private FileShim GetCachedDateFile()
        {
            if (_dir == null) _dir = GetCacheFolder();
            return _dir.File("LastNodeUpdate.cache", false);
        }


        //protected override string GetCacheFileName(string resource)
        //{
        //    if (_changed.IsBlank())
        //        return base.GetCacheFileName(resource);

        //    return $"{_changed}_{base.GetCacheFileName(resource)}";
        //}
    }


    public struct LastNodeUpdate
    {
        public string changed;
    }
}
