using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ErrH.Tools.Extensions;

namespace ErrH.Tools.DataErrorInfoHelpers
{
    public class DataErrorInfoHelper
    {
        private Dictionary<string, string> _errors = new Dictionary<string, string>();


        public string ErrorText => string.Join(L.f, _errors.Values);




        public string MessageIfBlank(string value, string label, string colName)
            => ErrorIf(value, x => x.IsBlank(),
                $"{label} must not be blank.", colName);


        public string MessageIfNoValue(DateTime? value, string label, string colName)
            => ErrorIf(value, x => !x.HasValue,
                $"{label} must have a value.", colName);


        public string MessageIfNull<T>(T value, string label, string colName)
            => ErrorIf(value, x => x == null,
                $"{label} must have a value.", colName);




        private string ErrorIf<T>(T value,
                                  Func<T, bool> func,
                                  string message,
                                  string colName)
        {
            if (!func.Invoke(value))
            {
                if (_errors.ContainsKey(colName))
                    _errors.Remove(colName);
                return null;
            }

            if (_errors.ContainsKey(colName))
                _errors[colName] = message;
            else
                _errors.Add(colName, message);

            return message;
        }
    }
}
