using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using ErrH.Tools.Extensions;

namespace ErrH.WpfTools.Converters
{
    public sealed class BlankToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Visibility.Collapsed;
            var str = value as string;
            if (str == null) str = value.ToString();
            return str.IsBlank() ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
