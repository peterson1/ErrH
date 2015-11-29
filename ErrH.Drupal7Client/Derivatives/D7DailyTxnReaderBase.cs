using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.Drupal7Models;
using ErrH.Tools.Extensions;
using ErrH.Tools.Loggers;

namespace ErrH.Drupal7Client.Derivatives
{
    public abstract class D7DailyTxnReaderBase<TStruct, TDto> 
        : LogSourceBase
        where TStruct : struct
    {
        private ID7Client             _client;
        private DailyTxn1Key<TStruct> _data;
        //private ISerializer           _serialr;
        //private FileShim              _cache;


        protected abstract string   _resourceURL { get; }
        public    abstract TStruct  ToStruct     (TDto item, out int keyID);


        public D7DailyTxnReaderBase (DailyTxn1Key<TStruct> dataArray
                                   , ID7Client d7Client)
        {
            _client  = ForwardLogs(d7Client);
            _data    = dataArray;
            //_serialr = serializer;
            //_cache   = DefineCacheFile(fsShim);
        }



        //private FileShim DefineCacheFile(IFileSystemShim fs)
        //{
        //    var dir = fs.GetSpecialDir(SpecialDir.LocalApplicationData);
        //    var d7c = _client.GetType().Name;
        //    var dom = _client.BaseUrl.TextAfter("//").Replace(":", "-");
        //    var usr = _client.CurrentUser.name;
        //    var fnm = typeof(TDto).Name + "." + typeof(TStruct).Name;
        //    var pat = fs.CombinePath(dir, d7c, dom, usr, fnm);
        //    return fs.File(pat);
        //}


        public async Task<bool> LoadTxnDay(DateTime date, CancellationToken token = new CancellationToken())
        {
            var url = _resourceURL.Slash(date.ToArg());
            var ret = await _client.Get<List<TDto>>(url, token);
            int key = 0;

            foreach (var item in ret)
                _data[date.Year, date.Month, date.Day, key]
                    = ToStruct(item, out key);

            return true;
        }

    }
}
