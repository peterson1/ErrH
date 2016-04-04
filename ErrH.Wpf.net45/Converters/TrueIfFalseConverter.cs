using System;
using System.Globalization;
using System.Windows.Data;

namespace ErrH.Wpf.net45.Converters
{
    public class TrueIfFalseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => !(bool)value;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
