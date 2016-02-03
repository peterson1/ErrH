using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using ErrH.Tools.Extensions;

namespace ErrH.WpfTools.Converters
{
    public sealed class CountToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Visibility.Collapsed;
            int count = 0;
            if (!int.TryParse(value.ToString(), out count)) return Visibility.Collapsed;
            return count > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
