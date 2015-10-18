using System.Windows;

namespace ErrH.WpfTools.Converters
{
    public sealed class VisibleIfFalseConverter : BooleanConverter<Visibility>
    {
        public VisibleIfFalseConverter() { }

        public VisibleIfFalseConverter(Visibility trueValue, Visibility falseValue) 
            : base(Visibility.Visible, Visibility.Collapsed){}
    }
}
