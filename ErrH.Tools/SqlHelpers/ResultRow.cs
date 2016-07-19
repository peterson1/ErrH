using System;
using System.Collections.Generic;
using System.Linq;
using ErrH.Tools.ErrorConstructors;
using ErrH.Tools.Extensions;

namespace ErrH.Tools.SqlHelpers
{
    public class ResultRow : Dictionary<string, object>
    {

        public string AsStr(int columnIndex)
            => Values.ToArray()[columnIndex].ToString();

        public string AsStr(string columnName)
            => Val(columnName).ToString();


        public char AsChar(int columnIndex, int charIndex = 0)
            => AsStr(columnIndex)[charIndex];


        public DateTime AsDate(int columnIndex)
            => DateTime.Parse(AsStr(columnIndex));


        public decimal GetDec(int columnIndex)
            => AsStr(columnIndex).ToDec();

        public decimal GetDec(string columnName)
            => Val(columnName).ToString().ToDec();


        public decimal? AsDec_(int columnIndex)
        {
            decimal d;
            return decimal.TryParse(AsStr(columnIndex), out d) 
                    ? d : (decimal?)null;
        }


        public int AsInt(int columnIndex)
            => AsStr(columnIndex).ToInt();

        public int AsInt(string columnName)
            => Val(columnName).ToString().ToInt();

        public int? AsInt_(string columnName)
        {
            int val;
            return int.TryParse(Val(columnName).ToString(), out val)
                ? val : (int?)null;
        }

        private object Val(string key)
        {
            object o;
            if (TryGetValue(key, out o)) return o;
            throw Error.NoMember(key);
        }
    }
}
