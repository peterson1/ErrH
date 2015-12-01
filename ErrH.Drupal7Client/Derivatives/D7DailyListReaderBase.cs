using System;
using System.Collections.Generic;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.Drupal7Models;
using ErrH.Tools.Serialization;

namespace ErrH.Drupal7Client.Derivatives
{
    public abstract class D7DailyListReaderBase<TDto, TStruct>
        : DailyReaderBase<TDto, TStruct>
        where TStruct : struct
    {
        private DailyList<TStruct> _data;


        protected abstract TStruct ToStruct(TDto dto);



        public D7DailyListReaderBase(DailyList<TStruct> dataArray
                                   , ID7Client d7Client
                                   , ISerializer serializer
            ) : base(d7Client, serializer)
        {
            _data = dataArray;
        }


        protected override bool AllocateMemory()
        {
            if (_data.IsAllocated) return true;
            try {
                _data.AllocateMemory(_startDate, _endDate);
            }
            catch (Exception ex) { return LogError("_data.AllocateMemory", ex); }
            return true;
        }


        protected override void ForEachD7Dto(TDto dto, DateTime date, HashSet<TStruct> hashSet)
        {
            hashSet.Add(ToStruct(dto));
            _data[date.Year, date.Month, date.Day] = hashSet;
        }
    }
}
