﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ErrH.Tools.ErrorConstructors;

namespace ErrH.Tools.Extensions
{
    public static class TypeExtensions
    {

        public static string ReadEmbedded(this Type typ
                                        , string resourcePath
                                        , string lineBreakReplacement = null
                                        , string tabReplacement = null)
        {
            try {
                using (Stream stream = typ.Assembly.GetManifestResourceStream(resourcePath))
                using (StreamReader readr = new StreamReader(stream))
                {
                    var s = readr.ReadToEnd();

                    s = lineBreakReplacement == null ? s
                         : s.Replace("\r\n", lineBreakReplacement);

                    return tabReplacement == null ? s
                         : s.Replace("\t", tabReplacement);
                }

            }
            catch (ArgumentNullException)
            {
                throw new Exception($"Unable to read “{resourcePath}”. Make sure that it's an embedded resource.");
            }
        }



        // http://stackoverflow.com/a/1398934
        public static bool IsNullableType(this Type type)
            => type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));



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
            bool inherit = false, bool errofIfMissing = false) 
                where T : Attribute
        {
            var atts = typ.GetCustomAttributes(typeof(T), inherit);
            if (atts.Length == 0)
            {
                if (!errofIfMissing) return default(T);
                Throw.NoMember<T>("class attribute");
            }
            return atts[0].As<T>();
        }


        public static IEnumerable<PropertyInfo> 
            PublicInstanceProps(this Type typ)
                => typ.GetProperties(BindingFlags.Public 
                                   | BindingFlags.Instance);

        public static IEnumerable<PropertyInfo> WritableProps(this Type typ)
            => typ.PublicInstanceProps().Where(x => x.CanWrite);
    }
}
