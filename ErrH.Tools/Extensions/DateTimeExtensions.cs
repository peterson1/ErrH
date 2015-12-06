using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace ErrH.Tools.Extensions
{
    public static class DateTimeExtensions
    {

        public static DateTime Yesterday(this DateTime today)
            => today.AddDays(-1);


        // thanks to http://stackoverflow.com/a/9176734/3973863
        public static IEnumerable<DateTime> EachDayUpTo(this DateTime start, DateTime end)
        {
            // Remove time info from start date (we only care about day). 
            DateTime currentDay = new DateTime(start.Year, start.Month, start.Day);
            while (currentDay <= end)
            {
                yield return currentDay;
                currentDay = currentDay.AddDays(1);
            }
        }


        public static string ToSqlArg(this DateTime date, string format = "yyyy-MM-dd HH:mm:ss")
            => $"'{date.ToArg(format)}'";

        public static string ToSqlArg(this DateTime? date, string format = "yyyy-MM-dd HH:mm:ss")
            => date.Value.ToSqlArg(format);

        public static string ToArg(this DateTime date, string format = "yyyy-MM-dd")
            => date.ToString(format);



        public static DateTime ToDate(this string text, string format)
        {
            return DateTime.ParseExact(text, format,
                        CultureInfo.InvariantCulture);
        }



        public static IEnumerable<DateTime> UpTo(this DateTime startDate,
                                DateTime endDate, int incrementDays = 1)
        {
            for (var day = startDate.Date;
                 day.Date <= endDate.Date;
                 day = day.AddDays(incrementDays)
            )
                yield return day;
        }



        /// <summary>
        /// from: http://stackoverflow.com/a/1248
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string TimeAgo(this DateTime date)
        {
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            var ts = new TimeSpan(DateTime.UtcNow.Ticks
                                - date.ToUniversalTime().Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 0)
            {
                return "not yet";
            }
            if (delta < 1 * MINUTE)
            {
                return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";
            }
            if (delta < 2 * MINUTE)
            {
                return "a minute ago";
            }
            if (delta < 45 * MINUTE)
            {
                return ts.Minutes + " minutes ago";
            }
            if (delta < 90 * MINUTE)
            {
                return "an hour ago";
            }
            if (delta < 24 * HOUR)
            {
                return ts.Hours + " hours ago";
            }
            if (delta < 48 * HOUR)
            {
                return "yesterday";
            }
            if (delta < 30 * DAY)
            {
                return ts.Days + " days ago";
            }
            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "one month ago" : months + " months ago";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "one year ago" : years + " years ago";
            }


        }
    }
}
