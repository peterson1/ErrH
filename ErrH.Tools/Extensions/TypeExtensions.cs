using System;

namespace ErrH.Tools.Extensions
{
    public static class TypeExtensions
    {

        public static bool IsNative(this Type type)
        {
            return type.Namespace.StartsWith("System");
        }


        public static string ListName(this Type type)
        {
            if (!type.IsGenericType) return type.Name;

            var list = type.FullName.TextBefore("`")
                                    .TextAfter(".", true);

            var item = type.FullName.TextAfter("[[")
                                    .TextBefore(",")
                                    .TextAfter(".", true);

            return list + item.Guillemet();
        }

        //public static bool IsCollection(this Type type)	{
        //	return type.GetInterfaces()
        //		.Contains(typeof(ICollection));	}


        public static T GetAttribute<T>(this Type typ,
            bool inherit = false) where T : Attribute
        {
            var atts = typ.GetCustomAttributes(typeof(T), inherit);
            if (atts.Length == 0) return default(T);
            return atts[0] as T;
        }

    }
}
