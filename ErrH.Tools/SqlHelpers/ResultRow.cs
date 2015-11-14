using System;
using System.Collections.Generic;
using System.Linq;

namespace ErrH.Tools.SqlHelpers
{
    public class ResultRow : Dictionary<string, object>
    {

        public string AsStr(int columnIndex)
            => Values.ToArray()[columnIndex].ToString();


        public char AsChar(int columnIndex, int charIndex = 0)
            => AsStr(columnIndex)[charIndex];


        public DateTime AsDate(int columnIndex)
            => DateTime.Parse(AsStr(columnIndex));


        public decimal AsDec(int columnIndex)
            => decimal.Parse(AsStr(columnIndex));


        public decimal? AsDec_(int columnIndex)
        {
            decimal d;
            return decimal.TryParse(AsStr(columnIndex), out d) 
                    ? d : (decimal?)null;
        }


        public int AsInt(int columnIndex)
            => int.Parse(AsStr(columnIndex));
    }
}
