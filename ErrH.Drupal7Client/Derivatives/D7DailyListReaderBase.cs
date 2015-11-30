using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ErrH.Tools.Authentication;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.Drupal7Models;
using ErrH.Tools.Extensions;
using ErrH.Tools.Loggers;
using ErrH.Tools.ScalarEventArgs;
using ErrH.Tools.Serialization;

namespace ErrH.Drupal7Client.Derivatives
{
    public abstract class D7DailyListReaderBase<TDto, TStruct>
        : LogSourceBase, IDailyReader<TDto, TStruct>
        where TStruct : struct
    {
        public event EventHandler<EArg<DateTime>> LoadedFromServer;

        private Dictionary<DateTime, string> _lastHashes = new Dictionary<DateTime, string>();
        private ID7Client                    _client;
        private DailyList<TStruct>           _data;
        private IBasicAuthenticationKey      _authKey;
        private ISerializer                  _serialr;

        protected abstract string    _resourceURL { get; }
        protected abstract DateTime  _startDate   { get; }
        protected abstract DateTime? _endDate     { get; }
        protected abstract TStruct   ToStruct     (TDto dto);



        public D7DailyListReaderBase(DailyList<TStruct> dataArray
                                   , ID7Client d7Client
                                   , IBasicAuthenticationKey credentials
                                   , ISerializer serializer)
        {
            _client  = ForwardLogs(d7Client);
            _data    = dataArray;
            _authKey = credentials;
            _serialr = serializer;
        }


        public async Task<bool> LoadTxnDay(DateTime date, CancellationToken token = new CancellationToken())
        {
            if (!_data.IsAllocated)
            {
                try {  _data.AllocateMemory(_startDate, _endDate);  }
                catch (Exception ex) { return LogError("_data.AllocateMemory", ex); }
            }

            var svrTask = ReadFromServer(date, token);
            var memTask = ReadFromMemory(date);

            await TaskEx.WhenAny(svrTask, memTask);

            return true;
        }



        private Task<bool> ReadFromMemory(DateTime date)
        {
            //if

            return true.ToTask();
        }


        private async Task<bool> ReadFromServer(DateTime date, CancellationToken token)
        {
            var url = _resourceURL.Slash(date.ToArg());
            List<TDto> d7r = null;

            if (!await StartSession(token)) return false;

            try { d7r = await _client.Get<List<TDto>>(url, token); }
            catch (Exception ex) { return LogError("_client.Get", ex); }
            if (d7r == null) return false;

            if (SameAsLast(d7r, date)) return true;

            var hSet = new HashSet<TStruct>();
            foreach (var dto in d7r)
            {
                hSet.Add(ToStruct(dto));
                _data[date.Year, date.Month, date.Day] = hSet;
            }

            LoadedFromServer?.Invoke(this, new EArg<DateTime> { Value = date });
            return true;
        }


        private bool SameAsLast(List<TDto> d7r, DateTime date)
        {
            var hash = _serialr.SHA1(d7r);

            if (!_lastHashes.ContainsKey(date))
            {
                _lastHashes.Add(date, hash);
                return false;
            }

            if (hash == _lastHashes[date])
                return Debug_n(_resourceURL.Slash(date.ToArg()), "Same hash as before.");

            _lastHashes[date] = hash;
            return false;
        }


        private async Task<bool> StartSession(CancellationToken token)
        {
            if (_client.IsLoggedIn) return true;

            if (!_authKey.UserName.IsBlank()
              && _authKey.Password.IsBlank())
                return Warn_n("LoginCfgFile does not include a password.",
                              "Please supply a password to login.");

            if (!_client.LocalizeSessionFile(_authKey)) return false;

            if (!_client.IsLoggedIn)
                if (_client.HasSavedSession) _client.LoadSession();

            if (_client.IsLoggedIn) return true;

            return await _client.Login(_authKey, token);
        }
    }
}
