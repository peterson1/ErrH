using System.Windows;
using ErrH.Tools.Extensions;

namespace ErrH.Wpf.net45.Converters
{
    public abstract class ZeroToVisibleConverterBase : ValueConverterBase
    {
        protected bool IsZeroOrNull(object value)
        {
            if (value == null) return true;
            if (!value.ToString().IsNumeric()) return false;
            var num = value.ToDec_();
            if (!num.HasValue) return true;
            if (num.Value == 0) return true;
            return false;
        }
    }

    public class VisibleIfZeroConverter : ZeroToVisibleConverterBase
    {
        protected override object ConvertValue(object value)
            => IsZeroOrNull(value) ? Visibility.Visible : Visibility.Collapsed;
    }

    public class VisibleIfNotZeroConverter : ZeroToVisibleConverterBase
    {
        protected override object ConvertValue(object value)
            => IsZeroOrNull(value) ? Visibility.Collapsed : Visibility.Visible;
    }
}
