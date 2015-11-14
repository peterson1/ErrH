using System;

namespace ErrH.Tools.SqlHelpers
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DbColAttribute : Attribute
    {
        public string ColumnName { get; }

        public DbColAttribute(string columnName)
        {
            ColumnName = columnName;
        }
    }
}
