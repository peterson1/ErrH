using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ErrH.Wpf.net45.Extensions
{
    public static class StyleExtensions
    {
        public static T? FindSetter<T>(this Style style, DependencyProperty property) where T : struct
        {
            if (style == null) return null;

            //if (style.Setters.Count == 0) return null;

            var setr = style.Setters.FirstOrDefault(x
                => ((Setter)x).Property == property) as Setter;

            if (setr != null) return (T?)setr.Value;

            if (style.BasedOn != null)
            {
                var val = style.BasedOn.FindSetter<T>(property);
                if (val.HasValue) return val;
            }
            return null;
        }

        public static void Set(this Style styl, FontWeight val)
            => styl.Setters.Add(new Setter(TextBlock.FontWeightProperty, val));


        public static void Set(this Style styl, FontStyle val)
            => styl.Setters.Add(new Setter(TextBlock.FontStyleProperty, val));


        public static void Set(this Style styl, VerticalAlignment val)
            => styl.Setters.Add(new Setter(TextBlock.VerticalAlignmentProperty, val));


        public static void Set(this Style styl, TextAlignment val)
            => styl.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, val));


        public static void Set(this Style styl, Brush val)
            => styl.Setters.Add(new Setter(TextBlock.ForegroundProperty, val));
    }
}
