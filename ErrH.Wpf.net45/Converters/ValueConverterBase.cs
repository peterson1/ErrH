using System;
using System.Globalization;
using System.Windows.Data;

namespace ErrH.Wpf.net45.Converters
{
    public abstract class ValueConverterBase : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => ConvertValue(value);

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        protected abstract object ConvertValue(object value);
    }
}
