namespace ErrH.WpfTools.Converters
{
    public sealed class TrueIfFalseConverter : BooleanConverter<bool>
    {
        public TrueIfFalseConverter() { }

        public TrueIfFalseConverter(bool trueValue, bool falseValue)
            :base (trueValue, falseValue) { }
    }
}
