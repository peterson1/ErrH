using System;
using System.Collections.Generic;

namespace ErrH.Tools.CollectionShims
{
    public class DailyList<T> : IDisposable where T : struct
    {
        private DateTime _startDate;
        private DateTime _endDate;

        public HashSet<T>[][][] Data;

        public bool IsAllocated { get; private set; }



        public void AllocateMemory(DateTime startDate, DateTime? endDate = null)
        {
            _startDate = startDate;
            _endDate   = endDate ?? DateTime.Now;

            int yrCount = (_endDate.Year - _startDate.Year) + 1;
            Data = new HashSet<T>[yrCount][][];

            for (int y = 0; y < Data.Length; y++)
            {
                Data[y] = new HashSet<T>[12][];

                for (int m = 0; m < Data[y].Length; m++)
                    Data[y][m] = new HashSet<T>[31];
            }
            IsAllocated = true;
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

            year  = year  - _startDate.Year;
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
