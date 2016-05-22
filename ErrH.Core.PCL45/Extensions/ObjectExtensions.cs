using System.Reflection;

namespace ErrH.Core.PCL45.Extensions
{
    public static class ObjectExtensions
    {
        public static object ValueByName(this object obj, string propertyName)
            => obj?.GetType().GetRuntimeProperty(propertyName).GetValue(obj);


        //public static bool HasProperty(this object obj, string propertyName)
        //{
        //    if (obj == null) return false;
        //    var propNames = obj.GetType().GetRuntimeProperties().Select(x => x.Name).ToList();
        //    return propNames.Contains(propertyName);
        //}


    }
}
