using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.Extensions;
using ErrH.Tools.FileSystemShims;
using ErrH.Tools.Serialization;

namespace ErrH.Drupal7Client.Derivatives
{
    public class D7BasicCacheReader : D7ServicesClient, ICacheSource
    {
        protected FolderShim _dir;

        protected string _cacheSubDir = "Basic Cache";

        public bool UseCachedFile { get; set; } = true;



        public D7BasicCacheReader(IFileSystemShim fsShim, ISerializer serializer) : base(fsShim, serializer)
        {
            LowRetryIntervalSeconds = -1;
        }


        public override async Task<T> Get<T>(string resource, CancellationToken cancelToken, string taskTitle, string successMsg, params Func<T, object>[] successMsgArgs)
        {
            T result = default(T);

            if (UseCachedFile)
            {
                try { result = await TryReadCache<T>(resource); }
                catch (Exception ex){ LogError("TryReadCache", ex); }
                if (result != null) return result;
            }

            try {
                result = await SendQuery(resource, cancelToken, taskTitle, successMsg, successMsgArgs, result);
            }
            catch (Exception ex){ LogError("base.Get<T>", ex); return default(T); }
            if (result == null) return result;


            if (UseCachedFile)
            {
                try { AddToCache(resource, result); }
                catch (Exception ex){ LogError("AddToCache", ex); }
            }

            return result;
        }


        private async Task<T> SendQuery<T>(string resource, CancellationToken cancelToken, string taskTitle, string successMsg, Func<T, object>[] successMsgArgs, T result) where T : new()
        {
            var baseTask = base.Get<T>(resource, cancelToken, taskTitle, successMsg, successMsgArgs);
            var othrTask = OtherTask(resource, cancelToken);

            if (othrTask == null) return await baseTask;

            try {
                await TaskEx.WhenAll(baseTask, othrTask);
            }
            catch (Exception ex){ LogError("TaskEx.WhenAll(baseTask, othrTask)", ex); }

            var baseResult = await baseTask;
            var othrResult = await othrTask;

            return baseResult;
        }


        protected virtual Task<bool> OtherTask(string resource, CancellationToken cancelToken)
        {
            return null;
        }


        protected virtual async Task<T> TryReadCache<T>(string resource) where T : new()
        {
            if (_dir == null) _dir = GetCacheFolder();
            var fName = GetCacheFileName(resource);
            if (!_dir.Has(fName)) return default(T);

            var file = _dir.File(fName);
            var ret = _serialzr.Read<T>(file);

            await TaskEx.Delay(1);
            return ret;
        }


        private void AddToCache<T>(string resource, T result) where T : new()
        {
            if (_dir == null) _dir = GetCacheFolder();

            string fName; try
            {
                fName = GetCacheFileName(resource);
            }
            catch (Exception ex){ LogError("GetCacheFileName", ex); return; }

            FileShim file; try
            {
                file = _dir.File(fName, false);
            }
            catch (Exception ex){ LogError("_dir.File", ex); return; }

            string json; try
            {
                json = _serialzr.Write(result, false);
            }
            catch (Exception ex) { LogError("_serialzr.Write", ex); return; }

            if (json.IsBlank())
            {
                Error_n("json.IsBlank", "");
                return;
            }

            if (file == null)
            {
                Error_n("file == null", "");
                return;
            }

            if (_fsShim == null)
            {
                Error_n("_fsShim == null", "");
                return;
            }

            string err = null; try {
                file.Write(json);
                //_fsShim.TryWriteFile(file.Path, out err, json, EncodeAs.UTF8);
            }
            catch (Exception ex)
            {
                Error_n("Failed to write file.", file?.Path + L.f + err);
                LogError("file.Write", ex);
            }
        }


        protected virtual string GetCacheFileName(string resource)
        {
            var s = resource.ToLower();
            s = s.Replace("/", "_");
            return s + ".json";
        }


        protected FolderShim GetCacheFolder()
        {
            var loc = _fsShim.GetSpecialDir(SpecialDir.LocalApplicationData);
            var typ = this.GetType().Name;
            var dom = _client.BaseUrl.TextAfter("//")
                                     .Replace(":", "-")
                                     .Replace("/", "");
            var usr = CurrentUser.name;//.Replace(" ", "_");
            var path = loc.Bslash(typ).Bslash(dom).Bslash(usr).Bslash(_cacheSubDir);
            return _fsShim.Folder(path);
        }


        public bool ClearCache(string filter = "*")
        {
            if (_dir == null) _dir = GetCacheFolder();
            var files = _dir?.Files(filter);
            if (files.Count == 0) return true;
            return files.All(x => x.Delete());
        }


        public bool HasCache(string filter = "*")
        {
            if (_dir == null) _dir = GetCacheFolder();
            return _dir?.Files(filter)?.Count > 0;
        }
    }
}
