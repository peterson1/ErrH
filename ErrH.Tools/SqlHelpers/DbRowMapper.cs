using System;
using System.Reflection;
using ErrH.Tools.ErrorConstructors;
using ErrH.Tools.Extensions;

namespace ErrH.Tools.SqlHelpers
{
    public class DbRowMapper
    {

        public static bool Map<T>(ResultRow source, 
            T target, IMapOverride mapOverride = null)
        {
            foreach (var kv in source)
            {
                var nme = kv.Key;
                var val = kv.Value;

                if (mapOverride?.HasOverride(nme) ?? false)
                    val = mapOverride.OverrideValue(nme, val);

                var ok = SetProperty(target, nme, val);
                if (!ok) return false;
            }
            return true;
        }


        private static bool SetProperty<T>(T target, 
            string propertyName, object value)
        {
            var prop = typeof(T).GetProperty(propertyName);
            if (prop == null) Throw.NoMember<T>(propertyName);

            //if (prop.PropertyType == typeof(int?))
            //{
            //    var cnv = Convert.ChangeType(value.ToInt(), typeof(int?), null);
            //    return SetMemberVal(target, cnv, prop);
            //}

            //if (value.GetType().Name == "DBNull")
            //    return SetMemberVal(target, null, prop);

            if (value is long)
                return SetMemberVal(target, value.ToInt(), prop);

            return SetMemberVal(target, value, prop);
        }



        private static bool SetMemberVal<T>(T target, object value, PropertyInfo prop)
        {
            try
            {
                prop.SetValue(target, value, null);
            }
            catch (Exception ex)
            {
                var msg = $"Can't cast value [ {value} ]" 
                        + $" of type ‹{value.GetType().Name}›" 
                        + $" to {prop.Name} ‹{prop.PropertyType.FullName}›.";
                throw new InvalidCastException(msg, ex);
            }
            return true;
        }
    }
}
