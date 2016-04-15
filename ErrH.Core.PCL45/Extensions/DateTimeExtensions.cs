using System;

namespace ErrH.Core.PCL45.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime IfEarlierThan (this DateTime date1, DateTime date2)
            => date1.Ticks < date2.Ticks ? date1 : date2;
    }
}
