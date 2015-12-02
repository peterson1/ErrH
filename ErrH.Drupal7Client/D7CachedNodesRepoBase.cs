using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ErrH.Tools.Authentication;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.Drupal7Models;
using ErrH.Tools.Drupal7Models.Entities;
using ErrH.Tools.Extensions;
using ErrH.Tools.FileSystemShims;
using ErrH.Tools.Serialization;

namespace ErrH.Drupal7Client
{
    public abstract class D7CachedNodesRepoBase<TNodeDto, TClass> 
        : D7NodesRepoBase<TNodeDto, TClass>, ICacheSource
        where TClass : ID7Node
    {
        private event EventHandler _cacheLoaded;

        private  IFileSystemShim _fs;
        private  ISerializer     _serialr;
        private  FileShim        _file;
        private  string          _subURL;

        protected string         _argPrefixForFilename = "";
        protected bool           _refreshCacheAfterLoad = true;
        protected int            _cacheRefreshSecondsDelay = 2;


        public bool UseCachedFile { get; set; } = true;



        public D7CachedNodesRepoBase(IFileSystemShim fileSystemShim, ISerializer serializer, ISessionClient client, IBasicAuthenticationKey credentials)
        {
            _fs      = fileSystemShim;
            _serialr = serializer;
            SetClient(client, credentials);

            _cacheLoaded += OnCacheLoaded_RefreshCache;
        }

        private async void OnCacheLoaded_RefreshCache(object sender, EventArgs e)
        {
            var m1 = $"Cache loaded for ‹{typeof(TClass).Name}›.";
            var m2 = _refreshCacheAfterLoad ? $"Will check for newer version in {_cacheRefreshSecondsDelay.x("second")}..."
                                             : "Cache refresh suspended.";
            Debug_n(m1, m2);

            await TaskEx.Delay(1000 * _cacheRefreshSecondsDelay);

            var tkn = new CancellationToken();
            if (!await SendCredentials(tkn)) return;

            if (_refreshCacheAfterLoad)
                await QueryThenCacheToFile(tkn);
        }


        public D7CachedNodesRepoBase(IFileSystemShim fileSystemShim, ISerializer serializer, IClientSource clientSource)
            : this(fileSystemShim, serializer, clientSource.Client, clientSource.AuthKey)
        {
        }



        /// <summary>
        /// First argument (args[0]) should always be the resource URL.
        /// </summary>
        /// <param name="tkn"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public override async Task<bool> LoadAsync(CancellationToken tkn, params object[] args)
        {
            _subURL = ParseArguments(args, out _file);
            if (_subURL.IsBlank()) return false;

            if (TryLoadCache())
            {
                _cacheLoaded?.Invoke(this, EventArgs.Empty);
                StartTrackingChanges();
                return true;
            }

            if (!await SendCredentials(tkn)) return false;
            return await QueryThenCacheToFile(tkn);
        }


        private async Task<bool> SendCredentials(CancellationToken tkn)
        {
            if (_client.IsLoggedIn) return true;
            if (!_client.LocalizeSessionFile(_credentials)) return false;

            Info_n($"Logging in as “{_credentials.UserName}”...", "");
            Debug_i($"server: {_credentials.BaseUrl}");
            if (_client.HasSavedSession) _client.LoadSession();
            if (_client.IsLoggedIn) return Debug_o("Loaded previously saved user session.");


            if (_credentials.Password.IsBlank())
                return Warn_n("Credentials did not include a password.",
                                "Please supply a password to login.");

            Debug_i($"Logging in to {_credentials.BaseUrl}...");
            if (!await _client.Login(_credentials, tkn)) return false;
            Debug_o($"Successfully logged in as “{_credentials.UserName}”.");

            return _client.IsLoggedIn;
        }


        private async Task<bool> QueryThenCacheToFile(CancellationToken tkn)
        {
            if (!await Query(_client, _subURL, tkn)) return false;

            var json = _serialr.Write(_list, false);
            var msg = $"‹{ClsTyp}› query: {_list.Count.x("record")} ({json.Length.KB()})";

            if (_file.Found && SameAs(_file, json, msg)) return true;


            string err;
            if (_fs.TryWriteFile(_file.Path, out err, json, EncodeAs.UTF8))
                return Debug_n($"New cache file created: “{_file.Name}”.", msg);
            else
                return Warn_n($"Failed to write cache file for ‹{ClsTyp}›.", err);
        }


        private bool SameAs(FileShim file, string json, string msg)
        {
            if (json.Length != file.ReadUTF8.Length)
            {
                RaiseDataChanged();
                return Warn_n($"Different size for result: {json.Length.KB()}.", msg);
            }

            if (json.SHA1() == file.SHA1)
                return Debug_n("Resulting file is same as cached content.", msg);
            else
            {
                RaiseDataChanged();
                return Warn_n("SHA-1 checksum differed from cached file.", msg);
            }
        }

        
        private async Task<bool> Query(ID7Client client, string subUrl, CancellationToken tkn)
        {
            Debug_n($"Querying repository of ‹{ClsTyp}›...", subUrl);

            List<TNodeDto> dtos;
            try
            {
                dtos = await client.Get<List<TNodeDto>>(subUrl, tkn);
            }
            catch (Exception ex)
            {
                return Error_n("Query error:", ex.Details(false, false));
            }

            if (dtos == null) return false;

            var tmp = dtos.Select(x => FromDto(x));
            if (!AreKeysUnique(tmp))
                return Warn_n($"‹{ClsTyp}› Keys are not unique.", GetKey.ToString());

            _list = tmp.ToList();
            if (_list.Count == 1 && _list[0] == null) _list.Clear();

            StartTrackingChanges();

            return true;
        }




        private string ParseArguments(object[] args, out FileShim cacheFile)
        {

            if (args.Length == 0 || !(args[0] is string))
            {
                cacheFile = null;
                return Error_("", $"Invalid argument for ‹{GetType().Name}›.LoadAsync()",
                                   "LoadAsync(args[0]) should be the resource URL");
            }
            var sufx = args.Length > 1 ? "_" + string.Join("_", args.Skip(1)) : "";
            cacheFile = DefineCacheFile(sufx);
            return string.Join("/", args); ;
        }


        private FolderShim DefineCacheFolder(ISessionClient client, 
                                             IBasicAuthenticationKey authKey)
        {
            //Throw.IfNull(_fs, "private FileSystemShim");
            //if (_fs == null) return null;

            if (authKey.BaseUrl.IsBlank() || authKey.UserName.IsBlank())
            {
                Warn_n("Unable to define cache folder.", 
                       "User name or base URL should not be blank.");
                return null;
            }
                

            var loc  = _fs.GetSpecialDir(SpecialDir.LocalApplicationData);
            var typ  = client.GetType().Name;
            var dom  = authKey.BaseUrl.TextAfter("//").Replace(":", "-");
            var usr  = authKey.UserName;//.Replace(" ", "_");
            var end  = "CachedNodes";
            var path = loc.Bslash(typ).Bslash(dom).Bslash(usr).Bslash(end);
            return _fs.Folder(path);
        }


        private FileShim DefineCacheFile(string suffix)
        {
            //var nme = $"{DtoTyp}{_argPrefixForFilename}{suffix}.json";
            var nme = GetCacheFileName(suffix);

            //var dir = _fs.GetSpecialDir(SpecialDir.LocalApplicationData)
            //                                      .Bslash(CACHE_FOLDER);
            //return _fs.File(dir.Bslash(nme));
            var foldr = DefineCacheFolder(_client, _credentials);
            if (foldr == null) return null;
            return foldr.File(nme, false);
        }


        private bool TryLoadCache()
        {
            if (!UseCachedFile) return false;
            if (_file == null) return false;

            if (!_file.Found)
                return Warn_n($"Missing cache ‹{DtoTyp}›.", _subURL);

            if (_serialr.TryRead(_file.ReadUTF8, out _list))
                return true;
                //return Debug_n($"Cache found for ‹{DtoTyp}›.", 
                //               $"Successfully parsed to List‹{ClsTyp}›.");
            else
                return Warn_n($"Cache found for ‹{DtoTyp}›.",
                              $"Failed to parse as List‹{ClsTyp}›.");
        }





        //public virtual bool ClearCache() => _file?.Delete() ?? false;

        public virtual bool ClearCache(string suffix)
        {
            var pattrn = GetCacheFileName(suffix);
            var foldr  = DefineCacheFolder(_client, _credentials);
            if (foldr == null) return false;
            var allDeleted = true;

            foreach (var file in foldr.Files(pattrn))
                if (!file.Delete()) allDeleted = false;

            return allDeleted;
        }


        public virtual bool HasCache(string suffix)
        {
            var pattrn = GetCacheFileName(suffix);
            var foldr = DefineCacheFolder(_client, _credentials);
            if (foldr == null) return false;

            return foldr.Files(pattrn).Count > 0;
        }



        private string GetCacheFileName(string suffix)
            => $"{DtoTyp}{_argPrefixForFilename}{suffix}.json";




        //{
        //    ParseArguments(args, out _file);
        //    return _file?.Delete() ?? false;
        //}


        private string DtoTyp => typeof(TNodeDto).Name;
        private string ClsTyp => typeof(TClass).Name;

    }
}
