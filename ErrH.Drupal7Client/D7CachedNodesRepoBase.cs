using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ErrH.Tools.Authentication;
using ErrH.Tools.ErrorConstructors;
using ErrH.Tools.Extensions;
using ErrH.Tools.FileSystemShims;
using ErrH.Tools.Serialization;

namespace ErrH.Drupal7Client
{
    public abstract class D7CachedNodesRepoBase<TNodeDto, TClass> : D7NodesRepoBase<TNodeDto, TClass>
    {
        private event EventHandler _cacheLoaded;

        private  IFileSystemShim _fs;
        private  ISerializer     _serialr;
        private  FileShim        _file;
        private  string          _subURL;
        protected bool           _refreshCacheAfterLoad = true;


        /// <summary>
        /// Inheriters may use this to append a parameter to the file.
        /// </summary>
        //protected virtual string FileSuffix => "";
        //protected abstract string SubURL { get; }


        public D7CachedNodesRepoBase(IFileSystemShim fileSystemShim, ISerializer serializer, ISessionClient client, IBasicAuthenticationKey credentials)
        {
            _fs      = fileSystemShim;
            _serialr = serializer;
            SetClient(client, credentials);

            _cacheLoaded += async (s, e) =>
            {
                if (_refreshCacheAfterLoad)
                    if (await CacheUpdated()) RaiseDataChanged();
            };
        }


        public async Task<bool> CacheUpdated()
        {
            var oldSize = _file.Size;
            var oldHash = _file.SHA1;

            //if (!await base.LoadAsync(new CancellationToken(), _subURL)) return false;
            if (!await SendQuery()) return false;
            if (!SaveCache(_list)) return false;

            if (_file.Size != oldSize) return true;
            return _file.SHA1 != oldHash;
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
                return true;
            }

            //if (!await base.LoadAsync(tkn, _subURL)) return false;
            if (!await SendQuery(tkn)) return false;

            return SaveCache(_list);
        }


        //private async Task<bool> SendCredentials(CancellationToken tkn)
        //{
        //
        //}


        private async Task<bool> SendQuery(CancellationToken tkn = new CancellationToken())
        {
            _client.LocalizeSessionFile(_credentials);

            if (!_client.IsLoggedIn)
                if (_client.HasSavedSession) _client.LoadSession();

            if (!_client.IsLoggedIn)
            {
                if (_credentials.Password.IsBlank())
                    return Warn_n("Credentials did not include a password.",
                                  "Please supply a password to login.");

                Debug_i($"Logging in to {_credentials.BaseUrl}...");
                if (!await _client.Login(_credentials, tkn)) return false;
                Debug_o($"Successfully logged in as “{_credentials.UserName}”.");
            }

            //_client.SaveSession();

            Debug_i($"List‹{DtoTyp}› from {_subURL}...");
            var dtos = await _client.Get<List<TNodeDto>>(_subURL, tkn);
            if (dtos == null) return false;
            Debug_o($"Query returned {dtos.Count.x("record")}.");

            _list = dtos.Select(x => FromDto(x)).ToList();
            if (_list.Count == 1 && _list[0] == null) _list.Clear();

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
            var nme = $"{DtoTyp}{suffix}.json";
            //var dir = _fs.GetSpecialDir(SpecialDir.LocalApplicationData)
            //                                      .Bslash(CACHE_FOLDER);
            //return _fs.File(dir.Bslash(nme));
            var foldr = DefineCacheFolder(_client, _credentials);
            if (foldr == null) return null;
            return foldr.File(nme, false);
        }


        private bool TryLoadCache()
        {
            if (_file == null) return false;

            if (!_file.Found)
                return Warn_n($"Cache missing for ‹{DtoTyp}›.", _file.Path);

            if (_serialr.TryRead(_file.ReadUTF8, out _list))
                return Debug_n($"Cache found for ‹{DtoTyp}›.", 
                               $"Successfully parsed to List‹{ClsTyp}›.");
            else
                return Warn_n($"Cache found for ‹{DtoTyp}›.",
                              $"Failed to parse as List‹{ClsTyp}›.");
        }


        private bool SaveCache(List<TClass> _list)
        {
            var json = _serialr.Write(_list, false);
            var ok   = _file.Write(json);

            string err;
            if (_fs.TryWriteFile(_file.Path, out err, json, EncodeAs.UTF8))
                return Debug_n($"Successfully cached List‹{ClsTyp}›.", 
                               $"{_list.Count.x("list item")}, {_file.Size.KB()}");
            else
                return Warn_n($"Failed to cache List‹{ClsTyp}›.", err);
        }


        private string DtoTyp => typeof(TNodeDto).Name;
        private string ClsTyp => typeof(TClass).Name;


    }
}
