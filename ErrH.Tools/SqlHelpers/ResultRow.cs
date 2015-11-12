using System;
using System.Collections.Generic;

namespace ErrH.Tools.SqlHelpers
{
    public class ResultRow : List<object>
    {
        public string AsStr(int columnIndex)
            => this[columnIndex].ToString();

        public DateTime AsDate(int columnIndex)
            => DateTime.Parse(this[columnIndex].ToString());

        public decimal AsDec(int columnIndex)
            => decimal.Parse(this[columnIndex].ToString());

        public decimal? AsDec_(int columnIndex)
        {
            decimal d;
            return decimal.TryParse(AsStr(columnIndex), out d) 
                    ? d : (decimal?)null;
        }

        public int AsInt(int columnIndex)
            => int.Parse(this[columnIndex].ToString());
    }
}
