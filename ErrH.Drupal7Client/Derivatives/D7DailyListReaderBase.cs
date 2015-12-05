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
        public DailyList<TStruct> Data { get; set; }

        protected abstract TStruct ToStruct(TDto dto, DateTime date);



        public D7DailyListReaderBase(ID7Client d7Client
                                   , ISerializer serializer
            ) : base(d7Client, serializer)
        {
        }


        protected override bool AllocateMemory()
        {
            if (Data.IsAllocated) return true;
            try {
                Data.AllocateMemory(_startDate, _endDate);
            }
            catch (Exception ex) { return LogError("_data.AllocateMemory", ex); }
            return true;
        }


        protected override void ForEachD7Dto(TDto dto, DateTime date, HashSet<TStruct> hashSet)
        {
            hashSet.Add(ToStruct(dto, date));
            Data[date.Year, date.Month, date.Day] = hashSet;
        }
    }
}
