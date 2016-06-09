using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ErrH.Core.PCL45.Extensions
{
    public static class TypeExtensions
    {
        /// <summary>
        /// from http://stackoverflow.com/a/13335824/3973863
        /// </summary>
        /// <param name="typeInfo"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetAllProperties(this Type type)
        {
            var typeInfo = type.GetTypeInfo();
            var list = typeInfo.DeclaredProperties.ToList();

            var subtype = typeInfo.BaseType;
            if (subtype != null)
                list.AddRange(subtype.GetAllProperties());

            foreach (var intrfce in typeInfo.ImplementedInterfaces)
                list.AddRange(intrfce.GetAllProperties());

            return list.ToArray();
        }
    }
}
