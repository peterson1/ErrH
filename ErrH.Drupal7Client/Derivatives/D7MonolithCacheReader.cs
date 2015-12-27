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
        private string _changed = null;

        public D7MonolithCacheReader(IFileSystemShim fsShim, ISerializer serializer) : base(fsShim, serializer)
        {
            _cacheSubDir = "Monolithic Cache";
        }



        protected override bool TryReadCache<T>(string resource, out T result)
        {
            _changed = GetLastUpdated(resource);
            if (_changed.IsBlank())
            {
                result = default(T);
                return false;
            }

            return base.TryReadCache<T>(resource, out result);
        }


        private string GetLastUpdated(string resource)
        {
            if (_dir == null) _dir = GetCacheFolder();
            var allF = _dir.Files();
            if (allF.Count == 0) return null;

            var fName = allF.ToList()
                            .OrderByDescending(x => x.Name);
            //dito palang
            return null;
        }


        protected override async Task<bool> OtherTask(string resource, CancellationToken cancelToken)
        {
            var req = new RequestShim(RESOURCE, RestMethod.Get);

            List<LastNodeUpdate> res; try
            {
                res = await _client.Send<List<LastNodeUpdate>>(req, cancelToken);
            }
            catch (Exception ex){ return LogError("base.Get<List<LastNodeUpdate>>", ex); }

            if (res == null)   return Error_n("res == null", "");
            if (res.Count > 1) return Error_n("res.Count > 1", "");

            _changed = res[0].changed.Replace(":", "_")
                                     .Replace(" ", "_");
            return true;
        }


        protected override string GetCacheFileName(string resource)
            => $"{_changed}_{base.GetCacheFileName(resource)}";
    }


    public struct LastNodeUpdate
    {
        public string changed;
    }
}
