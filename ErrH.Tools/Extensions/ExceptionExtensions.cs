using System;
using System.Linq;

namespace ErrH.Tools.Extensions
{
    public static class ExceptionExtensions
    {

        /// <summary>
        /// Returns the requested exception type from an aggregate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static T FromAggregate<T>(this Exception ex) where T : Exception
        {
            var aggErr = ex as AggregateException;
            if (aggErr == null) return default(T);

            foreach (var item in aggErr.InnerExceptions)
                if (item is T) return (T)item;

            return default(T);
        }


        //todo: rename to "Description"
        public static string Message(this Exception ex, bool withTypeNames = true, bool withShortStackTrace = true)
        {
            var msg = (" " + ex.Message + " ").Guillemets();
            var typ = (" " + ex.GetType().Name + " ").Guillemets();

            var inr = ex; var bullet = "";
            while (inr.InnerException != null)
            {
                inr = inr.InnerException;
                bullet += ".";

                msg += L.f + bullet + " " + inr.Message;
                typ += L.f + bullet + " " + inr.GetType().Name;
            }

            if (withTypeNames) msg += L.F + typ;

            if (withShortStackTrace)
                msg += L.F + ex.ShortStackTrace();

            return msg;
        }

        public static string Message(this AggregateException ex)
        {
            return string.Join(L.f,
                ex.InnerExceptions.Select(
                    x => ErrorLine(x)).ToArray());
        }


        private static string ErrorLine(Exception ex)
        {
            return "<" + ex.GetType().Name + "> (from AggregateException)"
                 + L.f + ex.Message();
        }


        public static string ShortStackTrace(this Exception ex)
        {
            try
            {
                return TrimPaths(ex.StackTrace);
            }
            catch (Exception)
            {
                return ex.StackTrace;
            }
        }

        public static string ShortStackTrace(this AggregateException ex)
        {
            return string.Join(L.f,
                ex.InnerExceptions.Select(x => x.ShortStackTrace()));
        }

        public static string TrimPaths(string stackTrace)
        {
            if (stackTrace == null) return "";
            if (stackTrace.IsBlank()) return "";

            var ss = stackTrace.Split('\n');

            for (int i = 0; i < ss.Length; i++)
            {
                var dropTxt = ss[i].Between(" in ", "\\", true);
                if (dropTxt != ss[i])
                    ss[i] = ss[i].Replace(dropTxt, "");
            }

            return string.Join("\n", ss);
        }
    }
}
