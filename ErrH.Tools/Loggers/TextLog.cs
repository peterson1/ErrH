using System;
using ErrH.Tools.Extensions;

namespace ErrH.Tools.Loggers
{
    public class TextLog
    {
        private static int _maxCol0 = 35;


        public static string Format(params object[] columns)
            => ShortDate(DateTime.Now) + AlignedColumns(columns);



        public static string AlignedColumns(object[] columns)
        {
            var s = "";
            if (columns.Length == 0) return s;

            var col = columns[0].ToString();
            if (col.Length > _maxCol0) _maxCol0 = col.Length;
            s = " " + col.AlignRight(_maxCol0);

            // if it got trimmed, show it fully on next column
            //if (col.Length > (_maxCol0 - 3)) s += $" | {col}";

            if (columns.Length == 1) return s;

            col = columns[1].ToString()
                            .Replace("\r", " ")
                            .Replace("\n", "_")
                            ;
            s += $" | {col}";
            if (columns.Length == 2) return s;

            return s;
        }


        public static string ShortDate(DateTime dateTime)
        {
            var sp = (dateTime.Month > 9 
                   && dateTime.Day > 9) ? "" : "-";

            var hr = dateTime.ToString("hh").ToInt();
            var ap = (hr > 9) ? "" : "t";

            var fmt = $"M{sp}d h:mm{ap}";

            return dateTime.ToString(fmt).ToLower();
        }
    }
}
