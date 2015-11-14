using ErrH.Tools.ErrorConstructors;

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

            if (prop == null)
                Throw.NoMember<T>(propertyName);

            prop.SetValue(target, value, null);
            return true;
        }
    }
}
