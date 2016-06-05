using System;
using ErrH.Core.PCL45.Extensions;

namespace ErrH.Core.PCL45.FormatProviders
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

            return Do(str.ToInt(), format);
        }


        public static string Do(int count, string singularPluralForms)
        {
            var str = count.ToString();
            if (count == 0) str = "No";

            if (singularPluralForms.Contains(";"))
            {
                var forms = singularPluralForms.Split(';');
                int i = (count == 1) ? 0 : 1;
                return str + " " + forms[i];
            }

            return str + " " + singularPluralForms
                + (count == 1 ? "" : "s");
        }

    }
}
