using System.Data.Common;
using System.Data.SQLite;

namespace ErrH.WpfRestClient.net45.DataAccess
{
    public partial class SQL
    {
        public static SqlClientBase Lite(string connectionStr)
            => new SQLite(connectionStr);
    }


    public class SQLite : SqlClientBase
    {
        public SQLite(string connectionStr) : base(connectionStr)
        {
        }

        protected override DbConnection CreateConnection(string connectionStr)
            => new SQLiteConnection(connectionStr);

        protected override DbCommand CreateCommand(string sqlQuery)
            => new SQLiteCommand(sqlQuery);
    }
}
