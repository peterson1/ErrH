using System;
using ErrH.Tools.Extensions;

namespace ErrH.Tools.FormatProviders
{
    public class PluralFormatProvider : IFormatProvider, ICustomFormatter
    {

        public object GetFormat(Type formatType)
        {
            return this;
        }


        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            var str = arg.ToString();

            if (format.IsBlank()) return str;

            if (!str.IsNumeric())
                return str + " " + format;

            var num = str.ToInt();
            if (num == 0) str = "No";

            if (format.Contains(";"))
            {
                var forms = format.Split(';');
                int i = (num == 1) ? 0 : 1;
                return str + " " + forms[i];
            }

            return str + " " + format
                + (num == 1 ? "" : "s");
        }

    }
}
