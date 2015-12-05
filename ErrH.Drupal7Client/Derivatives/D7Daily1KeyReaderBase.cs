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

        public IEnumerable<int> KeyIDs { get; set; }


        public DailyTxn1Key<TStruct> Data { get; set; }


        protected abstract TStruct ToStruct(TDto dto, out int keyID);


        public D7Daily1KeyReaderBase ( ID7Client d7Client
                                     , ISerializer serializer
        ) : base(d7Client, serializer)
        {
        }


        protected override bool AllocateMemory()
        {
            if (Data.IsAllocated) return true;


            if (KeyIDs == null || KeyIDs.Count() == 0)
                return Error_n("KeyIDs is empty.", "Please assign a value before anything else.");

            try {
                Data.AllocateMemory(KeyIDs, _startDate, _endDate);
            }
            catch (Exception ex) { return LogError("_data.AllocateMemory", ex); }
            return true;
        }


        protected override void ForEachD7Dto(TDto dto, DateTime date, HashSet<TStruct> hashSet)
        {
            int key = 0;
            var t = ToStruct(dto, out key);
            Data[date.Year, date.Month, date.Day, key] = t;
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
