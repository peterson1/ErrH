using System;
using System.Collections.Generic;

namespace ErrH.Tools.SqlHelpers
{
    public abstract class MapOverrideBase : IMapOverride
    {
        private Dictionary<string, Func<object, object>> 
            _dict = new Dictionary<string, Func<object, object>>();


        protected void Register(string propertyName, Func<object, object> overrideMethod)
            => _dict.Add(propertyName, overrideMethod);


        public bool HasOverride(string propertyName)
            => _dict.ContainsKey(propertyName);


        public object OverrideValue(string propertyName, object origValue)
            => _dict[propertyName].Invoke(origValue);
    }
}
