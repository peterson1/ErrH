using System.Collections.Generic;

namespace ErrH.Tools.Extensions
{
    public static class Dict
    {
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
    }
}
