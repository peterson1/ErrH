using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.Drupal7Models;
using ErrH.Tools.Extensions;
using ErrH.Tools.Loggers;
using ErrH.Tools.ScalarEventArgs;
using ErrH.Tools.Serialization;

namespace ErrH.Drupal7Client.Derivatives
{
    public abstract class DailyReaderBase<TIn, TOut> : LogSourceBase, IDailyReader<TIn, TOut> where TOut : struct
    {
        public event EventHandler<EArg<DateTime>> LoadedFromServer;

        protected Dictionary<DateTime, string> _lastHashes = new Dictionary<DateTime, string>();
        protected ID7Client                    _client;
        protected ISerializer                  _serialr;

        protected abstract string     _resourceURL   { get; }
        protected abstract DateTime   _startDate     { get; }
        protected abstract DateTime?  _endDate       { get; }
        protected abstract bool       AllocateMemory ();
        protected abstract void       ForEachD7Dto   (TIn dto, DateTime date
                                                    , HashSet<TOut> hashSet);



        public DailyReaderBase(ID7Client d7Client
                             , ISerializer serializer)
        {
            _client  = d7Client;
            _serialr = serializer;
        }



        public async Task<bool> LoadTxnDay(DateTime date, CancellationToken token = new CancellationToken())
        {
            if (!AllocateMemory()) return false;
            //var svrTask = ReadFromServer(date, token);
            //var memTask = ReadFromMemory(date);
            //await TaskEx.WhenAny(svrTask, memTask);
            //return true;
            return await ReadFromServer(date, token);
        }



        protected virtual Task<bool> ReadFromMemory(DateTime date)
        {
            return true.ToTask();
        }


        protected virtual async Task<bool> ReadFromServer(DateTime date, CancellationToken token)
        {
            var url = _resourceURL.Slash(date.ToArg());

            if (!_client.IsLoggedIn)
                return Error_n("D7Client is not logged in.", "");

            //Debug_n("Reading from server...", url);
            List<TIn> d7r = null;
            try
            {
                d7r = await _client.Get<List<TIn>>(url, token);
            }
            catch (Exception ex) { return LogError("_client.Get", ex); }
            if (d7r == null) return false;

            if (d7r.Count == 0)
                Warn_n(RecCount(d7r), url);
            else
                Info_n(RecCount(d7r), url);

            if (SameAsLast(d7r, date)) return true;

            var hSet = new HashSet<TOut>();
            foreach (var dto in d7r)
            {
                try
                {
                    ForEachD7Dto(dto, date, hSet);
                }
                catch (Exception ex) { return LogError($"‹{GetType().Name}›.ForEachD7Dto", ex); }
            }

            RaiseLoadedFromServer(date);
            return true;
        }


        private string RecCount(List<TIn> d7r)
            => $"{d7r.Count.WithComma().AlignRight(6)} x ‹{typeof(TIn).Name}›";


        public void RaiseLoadedFromServer(DateTime date)
            => LoadedFromServer?.Invoke(this, new EArg<DateTime> { Value = date });



        //private async Task<bool> StartSession(CancellationToken token)
        //{
        //    if (_client.IsLoggedIn) return true;

        //    if (!_authKey.UserName.IsBlank()
        //      && _authKey.Password.IsBlank())
        //        return Warn_n("LoginCfgFile does not include a password.",
        //                      "Please supply a password to login.");

        //    if (!_client.LocalizeSessionFile(_authKey)) return false;

        //    if (!_client.IsLoggedIn)
        //        if (_client.HasSavedSession) _client.LoadSession();

        //    if (_client.IsLoggedIn) return true;

        //    return await _client.Login(_authKey, token);
        //}






        private bool SameAsLast(List<TIn> d7r, DateTime date)
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

    }
}
