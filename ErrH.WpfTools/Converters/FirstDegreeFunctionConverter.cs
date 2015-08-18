using System;
using System.Windows.Data;

namespace ErrH.WpfTools.Converters
{
    /// <summary>
    /// Will return a*value + b
    /// from http://stackoverflow.com/a/4973289/3973863
    /// </summary>
    public class FirstDegreeFunctionConverter : IValueConverter
    {
        public double A { get; set; }
        public double B { get; set; }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double a = GetDoubleValue(parameter, A);

            double x = GetDoubleValue(value, 0.0);

            return (a * x) + B;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double a = GetDoubleValue(parameter, A);

            double y = GetDoubleValue(value, 0.0);

            return (y - B) / a;
        }

        #endregion


        private double GetDoubleValue(object parameter, double defaultValue)
        {
            double a;
            if (parameter != null)
                try
                {
                    a = System.Convert.ToDouble(parameter);
                }
                catch
                {
                    a = defaultValue;
                }
            else
                a = defaultValue;
            return a;
        }
    }
}
