using System.Collections.Generic;
using System.Reflection;
using ErrH.Tools.ErrorConstructors;
using ErrH.Tools.Extensions;

namespace ErrH.Tools.SqlHelpers
{
    public class SqlBuilder
    {
        public static string SELECT<T>()// where T : IOrigFromDb
        {
            var tbl  = GetTableAttrib<T>();
            var cols = JoinedColNames<T>();
            if (tbl == null || cols == null) return null;

            return $"SELECT {cols} FROM {tbl.TableName}"
                 + $" ORDER BY {tbl.KeyColumn};";
        }


        public static string SELECT_ByKey<T>(int recordKeyID)
        {
            var tbl = GetTableAttrib<T>();
            var cols = JoinedColNames<T>();
            if (tbl == null || cols == null) return null;

            return $"SELECT {cols} FROM {tbl.TableName}" 
                 + $" WHERE {tbl.KeyColumn} = {recordKeyID}"
                 + $" ORDER BY {tbl.KeyColumn};";
        }


        private static string JoinedColNames<T>()
        {
            var colNames = new List<string>();
            foreach (var prop in Props<T>())
            {
                var colAtt = prop.GetAttribute<DbColAttribute>();
                if (colAtt != null)
                {
                    var cNme = colAtt.ColumnName;

                    if (cNme.ToLower().Contains(" as "))
                        Throw.BadArg($"‹{typeof(T).Name}› DbCol for {prop.Name}", 
                            "should not contain keyword “AS”");

                    var s = $"{cNme} AS {prop.Name}";
                    colNames.Add(s);
                }
            }
            return string.Join(", ", colNames);
        }


        public static DbTableAttribute GetTableAttrib<T>()
            => typeof(T).GetAttribute<DbTableAttribute>();


        private static PropertyInfo[] Props<T>()
            => typeof(T).GetProperties(BindingFlags.Instance 
                                     | BindingFlags.Public);
    }
}
