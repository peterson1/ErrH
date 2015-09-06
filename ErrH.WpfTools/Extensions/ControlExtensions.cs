using System;
using System.Windows.Controls;
using ErrH.Tools.ErrorConstructors;
using ErrH.Tools.Extensions;

namespace ErrH.WpfTools.Extensions
{
    public static class ControlExtensions
    {
        public static T Find<T>(this Control parent, string xName) where T : class
        {
            object ctrl = null;
            var m1 = $"Unable to find x:Name=“{xName}” in {parent.GetType().Name}." + L.F;

            try { ctrl = parent.Template.FindName(xName, parent); }
            catch (Exception ex)
                { throw new ArgumentException(m1 + "Error in Template.FindName()", ex); }

            if (ctrl == null)
                throw new ArgumentException(m1 + "Template.FindName() returned NULL.");

            try { return ctrl as T; }
            catch (Exception)
                { throw Error.BadCast<T>(ctrl); }
        }
    }
}
