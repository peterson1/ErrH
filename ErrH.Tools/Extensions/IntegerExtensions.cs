using System;

namespace ErrH.Tools.Extensions
{
    public static class IntegerExtensions
    {

        public static string WithComma
            (this int number, string thousandsSeparator = ",")
        {
            return number.ToString("#,#0");
        }


        //public static string x
        //	(this int number, string singularNoun)
        //{
        //	if (number == 1)
        //		return "1 " + singularNoun;
        //	else
        //		return (number == 0) ? "No " + singularNoun + "s"
        //							 : number + " " + singularNoun + "s";
        //}


        public static string KB(this long byteCount)
        {
            return BytesToString(byteCount);
        }

        public static string KB(this int byteCount)
        {
            return BytesToString(byteCount);
        }

        
        /// <summary>
        /// Performs the action x times.
        /// </summary>
        /// <param name="repetitions"></param>
        /// <param name="action"></param>
        public static void Loop(this int repetitions, Action action)
        {
            for (int i = 1; i <= repetitions; i++)
                action.Invoke();
        }


        // http://stackoverflow.com/a/4975942/3973863
        private static string BytesToString(long byteCount)
        {
            string[] suf = { " B",
                         " KB",
                         " MB",
                         " GB",
                         " TB",
                         " PB",
                         " EB" }; //Longs run out around EB
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + suf[place];
        }

    }
}
