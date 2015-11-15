using System;

namespace ErrH.Tools.SqlHelpers
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DbTableAttribute : Attribute
    {
        public string TableName { get; }
        public string KeyColumn { get; }


        public DbTableAttribute(string tableName, string keyColName)
        {
            TableName = tableName;
            KeyColumn = keyColName;
        }
    }
}
