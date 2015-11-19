using System;
using System.Reflection;
using ErrH.Tools.Extensions;

namespace ErrH.Tools.SqlHelpers
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DbColAttribute : Attribute
    {
        public PropertyInfo  Property    { get; internal set; }
        public bool          IsKey       { get; set; }

        //todo: don't use this
        public bool          IsHash      { get; set; }

        public string        ColumnName  { get; }


        public DbColAttribute(string columnName)
        {
            ColumnName = columnName;
        }


        public static DbColAttribute Key<T>()
            => FindAtt<T>(x => x.IsKey == true);


        public static DbColAttribute Hash<T>()
            => FindAtt<T>(x => x.IsHash == true);



        private static DbColAttribute FindAtt<T>(Func<DbColAttribute, bool> filter)
        {
            foreach (var prop in typeof(T).PublicInstanceProps())
            {
                var colAtt = prop.GetAttribute<DbColAttribute>();
                if (colAtt != null)
                {
                    if (filter(colAtt))
                    {
                        colAtt.Property = prop;
                        return colAtt;
                    }
                }
            }
            return null;
        }
    }
}
