using System.Windows.Input;

namespace ErrH.Wpf.net45.Extensions
{
    public static class KeyExtensions
    {
        public static bool IsAlphabet(this Key key)
            => key >= Key.A && key <= Key.Z;
    }
}
