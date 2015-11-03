using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace ErrH.WpfTools.Converters
{
    public sealed class TrueIfNoItemsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => !HasItems(value);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => HasItems(value);

        private bool HasItems(object value)
        {
            var list = value as IEnumerable;
            if (list == null) return false;

            foreach (var item in list)
            {
                return true;
            }

            return false;
        }
    }
}
