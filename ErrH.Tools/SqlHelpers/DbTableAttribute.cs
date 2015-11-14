using System;

namespace ErrH.Tools.SqlHelpers
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DbTableAttribute : Attribute
    {
        public string TableName { get; set; }
        public string KeyColumn { get; set; }


        public DbTableAttribute(string tableName, string keyColName)
        {
            TableName = tableName;
            KeyColumn = keyColName;
        }
    }
}
