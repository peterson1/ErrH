using System.Data.Common;
using System.Data.SqlClient;

namespace ErrH.WpfRestClient.net45.DataAccess
{
    public partial class SQL
    {
        public static SqlClientBase Server(string connectionStr)
            => new SqlServer(connectionStr);
    }


    public class SqlServer : SqlClientBase
    {
        public SqlServer(string connectionStr) : base(connectionStr)
        {
        }


        protected override DbConnection CreateConnection(string connectionStr)
            => new SqlConnection(connectionStr);

        protected override DbCommand CreateCommand(string sqlQuery)
            => new SqlCommand(sqlQuery);
        
        //public static async Task<string> ChecksumSHA1
        //    (string checksumQuery, string connectionStr)
        //{
        //    var hashes = new List<int>();

        //    using (var conn = new SqlConnection(connectionStr))
        //    using (var cmd = new SqlCommand())
        //    {
        //        cmd.Connection  = conn;
        //        cmd.CommandText = checksumQuery;

        //        await conn.OpenAsync();
        //        var readr = await cmd.ExecuteReaderAsync();

        //        while (readr.Read())
        //            hashes.Add(readr.GetInt32(0));

        //        conn.Close();
        //    }
        //    if (hashes.Count == 0) return null;

        //    return string.Join("", hashes).SHA1();
        //}


        //private static async Task FillList<T>
        //    (List<T> list, Func<SqlDataReader, T> function,
        //    string sqlQuery, string connectionStr)
        //{
        //    using (var conn = new SqlConnection(connectionStr))
        //    using (var cmd = new SqlCommand())
        //    {
        //        cmd.Connection = conn;
        //        cmd.CommandText = sqlQuery;

        //        await conn.OpenAsync();
        //        var readr = await cmd.ExecuteReaderAsync();

        //        while (readr.Read())
        //            list.Add(function(readr));

        //        conn.Close();
        //    }
        //}
    }
}
