using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ErrH.Tools.ErrorConstructors;

namespace ErrH.Tools.Extensions
{
    public static class Dict
    {

        public static T MapTo<T>(this IDictionary dict, T obj = default(T)) 
            where T : new()
        {
            if (obj == null) obj  = new T();
            var props = typeof(T).PublicInstanceProps();

            foreach (string key in dict.Keys)
            {
                var prop = props.FirstOrDefault(x => x.Name == key);
                if (prop != null)
                    prop.SetValue(obj, dict[key], null);
            }
            return obj;
        }


        /// <summary>
        /// Creates a new Dictionary object, and adds the parameters as its first key-value pair.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> onary<TKey, TValue>(TKey key, TValue value)
        {
            var d = new Dictionary<TKey, TValue>();
            d.Add(key, value);
            return d;
        }

        public static int AsInt(this IDictionary dict, 
            string key, bool errorIfNotFound = false)
                => Val(dict, key, errorIfNotFound).ToString().ToInt();


        private static object Val(IDictionary dict, 
            string key, bool errorIfNotFound = false)
        {
            if (dict.Contains(key)) return dict[key];
            if (!errorIfNotFound) return null;
            throw Error.NoMember(key);
        }
    }
}
