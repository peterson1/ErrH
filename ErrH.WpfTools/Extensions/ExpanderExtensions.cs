using System.Windows.Controls;

namespace ErrH.WpfTools.Extensions
{
    public static class ExpanderExtensions
    {
        public static void Toggle(this Expander exp)
            => exp.IsExpanded = !exp.IsExpanded;
    }
}
