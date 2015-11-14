using System;
using System.Reflection;

namespace ErrH.Tools.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static T GetAttribute<T>(this PropertyInfo prop, 
            bool inherit = false) where T : Attribute
        {
            var atts = prop.GetCustomAttributes(typeof(T), inherit);
            if (atts.Length == 0) return default(T);
            return atts[0] as T;
        }
    }
}
