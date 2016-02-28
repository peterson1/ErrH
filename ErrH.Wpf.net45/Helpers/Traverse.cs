using System.Windows.Input;

namespace ErrH.Wpf.net45.Helpers
{
    public class Traverse
    {
        public static TraversalRequest Down     => new TraversalRequest(FocusNavigationDirection.Down);
        public static TraversalRequest First    => new TraversalRequest(FocusNavigationDirection.First);
        public static TraversalRequest Last     => new TraversalRequest(FocusNavigationDirection.Last);
        public static TraversalRequest Left     => new TraversalRequest(FocusNavigationDirection.Left);
        public static TraversalRequest Next     => new TraversalRequest(FocusNavigationDirection.Next);
        public static TraversalRequest Previous => new TraversalRequest(FocusNavigationDirection.Previous);
        public static TraversalRequest Right    => new TraversalRequest(FocusNavigationDirection.Right);
        public static TraversalRequest Up       => new TraversalRequest(FocusNavigationDirection.Up);
    }
}
