using System;
using System.Globalization;
using System.Windows.Data;

namespace ErrH.Wpf.net45.Converters
{
    public class FalseIfHasItemsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => HasItems(value) ? false : true;


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private bool HasItems(object value) 
            => VisibleIfHasItemConverter.HasItems(value);
    }
}
