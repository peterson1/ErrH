﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ErrH.Tools.CollectionShims
{
    public class DailyTxn1Key<T> : IDisposable where T : struct
    {
        private DateTime _startDate;
        private DateTime _endDate;
        private int[]    _keyIDs;

        public bool IsAllocated { get; private set; }

        public Nullable<T>[][][][] Data;



        public void AllocateMemory(IEnumerable<int> completeKeyIDs, DateTime startDate, DateTime? endDate = null)
        {
            _startDate = startDate;
            _endDate   = endDate ?? DateTime.Now;
            _keyIDs    = completeKeyIDs.ToArray();

            int yrCount = (_endDate.Year - _startDate.Year) + 1;
            Data = new Nullable<T>[yrCount][][][];

            for (int y = 0; y < Data.Length; y++)
            {
                Data[y] = new Nullable<T>[12][][];

                for (int m = 0; m < Data[y].Length; m++)
                {
                    Data[y][m] = new Nullable<T>[31][];

                    for (int d = 0; d < Data[y][m].Length; d++)
                        Data[y][m][d] = new Nullable<T>[_keyIDs.Length];
                }
            }
            IsAllocated = true;
        }



        public IEnumerable<KeyValuePair<int, T>> this[int year, int month, int day]
        {
            get
            {
                var y = year;
                var m = month;
                var d = day;
                ToIndeces(ref y, ref m, ref d);

                var ret = new List<KeyValuePair<int, T>>();

                for (int i = 0; i < Data[y][m][d].Length; i++)
                {
                    if (Data[y][m][d][i].HasValue)
                        ret.Add(new KeyValuePair<int, T>
                            (_keyIDs[i], Data[y][m][d][i].Value));
                }
                return ret;
            }
        }


        public T this[int year, int month, int day, int keyID]
        {
            get { return Get(year, month, day, keyID).Value; }
            set {        Set(year, month, day, keyID, value);       }
        }


        private Nullable<T> Get(int year, int month, int day, int key)
        {
            var y = year;
            var m = month;
            var d = day;
            var k = key;
            ToIndeces(ref y, ref m, ref d, ref k);
            var ret = Data[y][m][d][k];

            if (!ret.HasValue) throw new IndexOutOfRangeException
                    ($"No value was set for {year}-{month}-{day} [{key}].");

            return ret;
        }

        private void Set(int year, int month, int day, int key, T value)
        {
            ToIndeces(ref year, ref month, ref day, ref key);
            Data[year][month][day][key] = value;
        }




        private void ToIndeces(ref int year, ref int month, ref int day, ref int key)
        {
            if (!IsAllocated)
                throw new InvalidOperationException($"{GetType().Name} :  Please call {nameof(AllocateMemory)}() first. ");

            var i = Array.IndexOf<int>(_keyIDs, key);

            if (i == -1)
                throw new IndexOutOfRangeException($"Key ‹{key}› not found in list of allocated keys.");

            key = i;
            ToIndeces(ref year, ref month, ref day);
        }


        private void ToIndeces(ref int year, ref int month, ref int day)
        {
            year  = year - _startDate.Year;
            month = month - 1;
            day   = day - 1;
        }


        public void Dispose()
        {
            Array.Clear(Data, 0, Data.Length);
            IsAllocated = false;
        }
    }
}