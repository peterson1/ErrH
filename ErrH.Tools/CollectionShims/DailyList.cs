using System;
using System.Collections.Generic;
using ErrH.Tools.Loggers;

namespace ErrH.Tools.CollectionShims
{
    public class DailyList<T> : LogSourceBase, IDisposable where T : struct
    {
        public HashSet<T>[][][] Data        { get; set; }
        public DateTime         StartDate   { get; set; }
        public DateTime         EndDate     { get; set; } = DateTime.Now;
        public bool             IsAllocated { get; set; }



        public void AllocateMemory(DateTime startDate, DateTime? endDate = null)
        {
            StartDate = startDate;
            EndDate   = endDate ?? DateTime.Now;

            int yrCount = (EndDate.Year - StartDate.Year) + 1;
            Data = new HashSet<T>[yrCount][][];

            for (int y = 0; y < Data.Length; y++)
            {
                Data[y] = new HashSet<T>[12][];

                for (int m = 0; m < Data[y].Length; m++)
                    Data[y][m] = new HashSet<T>[31];
            }
            IsAllocated = true;
        }


        public HashSet<T> this[DateTime date]
            //=> this[date.Year, date.Month, date.Day];
        {
            get
            {
                if (date < StartDate || date > EndDate) return null;
                return this[date.Year, date.Month, date.Day];
            }
        }


        public HashSet<T> this[int year, int month, int day]
        {
            get { return Get(year, month, day); }
            set {        Set(year, month, day, value); }
        }


        private HashSet<T> Get(int year, int month, int day)
        {
            var y = year;
            var m = month;
            var d = day;
            ToIndeces(ref y, ref m, ref d);
            return Data[y][m][d];
        }


        private void Set(int year, int month, int day, HashSet<T> value)
        {
            ToIndeces(ref year, ref month, ref day);
            Data[year][month][day] = value;
        }



        private void ToIndeces(ref int year, ref int month, ref int day)
        {
            if (!IsAllocated)
                throw new InvalidOperationException
                    ($"{GetType().Name} :  Please call {nameof(AllocateMemory)}() first. ");

            year  = year  - StartDate.Year;
            month = month - 1;
            day   = day   - 1;
        }



        public void Dispose()
        {
            Array.Clear(Data, 0, Data.Length);
            IsAllocated = false;
        }
    }
}
