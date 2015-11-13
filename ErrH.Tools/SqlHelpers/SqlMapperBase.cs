using System;

namespace ErrH.Tools.SqlHelpers
{
    public class SqlMapperBase
    {
        public virtual string TableName { get; set; }
        public virtual string TableKey  { get; set; }



        public virtual string SELECT1_sql(int recordID)
            => $"SELECT * FROM {TableName} WHERE {TableKey} = {recordID}";



        public virtual void CopyValues(ResultRow row)
        {
            throw new NotImplementedException();
        }
    }
}
