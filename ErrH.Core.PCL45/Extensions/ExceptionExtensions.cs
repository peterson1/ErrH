using System;
using System.Linq;

namespace ErrH.Core.PCL45.Extensions
{
    public static class ExceptionExtensions
    {
        public static string Details(this Exception ex, bool withTypeNames = true, bool withShortStackTrace = true)
            => FormatError(ex, withTypeNames, withShortStackTrace);

        private static string FormatError(Exception ex, bool withTypeNames, bool withShortStackTrace)
        {
            var msg = ex.Message;
            var typ = $"‹{ex.GetType().Name}›";

            var inr = ex; var bullet = "";
            while (inr.InnerException != null)
            {
                inr = inr.InnerException;
                bullet += ".";

                msg += L.f + bullet + " " + inr.Message;
                typ += L.f + bullet + " " + inr.GetType().Name;
            }

            if (withTypeNames) msg += L.f + typ;

            if (withShortStackTrace)
                msg += L.f + ex.ShortStackTrace();

            return msg;
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
