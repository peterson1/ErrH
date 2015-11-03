using System;
using System.Globalization;
using System.Windows.Data;

namespace ErrH.WpfTools.Converters
{
    public sealed class TrueIfNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value == null;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => value != null;
    }
}
