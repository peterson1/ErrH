using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ErrH.Wpf.net45.Converters
{
    public class VisibleIfHasItemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => HasItems(value) ? Visibility.Visible : Visibility.Collapsed;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => HasItems(value) ? Visibility.Collapsed : Visibility.Visible;


        internal static bool HasItems(object value)
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
