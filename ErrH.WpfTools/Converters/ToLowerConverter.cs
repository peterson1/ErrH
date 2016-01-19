using System;
using System.Globalization;
using System.Windows.Data;
using ErrH.Tools.Extensions;

namespace ErrH.WpfTools.Converters
{
    public class ToLowerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value.ToString();
            if (str.IsBlank()) return "";
            return str.ToLower();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
