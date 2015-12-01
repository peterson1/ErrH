using System;
using System.Collections.Generic;
using System.Linq;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.Drupal7Models;
using ErrH.Tools.Serialization;

namespace ErrH.Drupal7Client.Derivatives
{
    public abstract class D7Daily1KeyReaderBase<TDto, TStruct> 
        : DailyReaderBase<TDto, TStruct>
        where TStruct : struct
    {
        private DailyTxn1Key<TStruct> _data;


        public IEnumerable<int> KeyIDs { get; set; }


        protected abstract TStruct ToStruct(TDto dto, out int keyID);


        public D7Daily1KeyReaderBase (DailyTxn1Key<TStruct> dataArray
                                   , ID7Client d7Client
                                   , ISerializer serializer
            ) : base(d7Client, serializer)
        {
            _data = dataArray;
        }


        protected override bool AllocateMemory()
        {
            if (_data.IsAllocated) return true;


            if (KeyIDs == null || KeyIDs.Count() == 0)
                return Error_n("KeyIDs is empty.", "Please assign a value before anything else.");

            try {
                _data.AllocateMemory(KeyIDs, _startDate, _endDate);
            }
            catch (Exception ex) { return LogError("_data.AllocateMemory", ex); }
            return true;
        }


        protected override void ForEachD7Dto(TDto dto, DateTime date, HashSet<TStruct> hashSet)
        {
            int key = 0;

            _data[date.Year, date.Month, date.Day, key]
                = ToStruct(dto, out key);
        }


        //public override async Task<bool> LoadTxnDay(DateTime date, CancellationToken token = new CancellationToken())
        //{
        //    var url = _resourceURL.Slash(date.ToArg());
        //    var ret = await _client.Get<List<TDto>>(url, token);
        //    int key = 0;

        //    foreach (var item in ret)
        //        _data[date.Year, date.Month, date.Day, key]
        //            = ToStruct(item, out key);

        //    return true;
        //}

    }
}
