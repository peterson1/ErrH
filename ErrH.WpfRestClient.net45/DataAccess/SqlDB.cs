using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using ErrH.Tools.Extensions;

namespace ErrH.WpfRestClient.net45.DataAccess
{
    public class SqlDB
    {
        public static async Task<string> ChecksumSHA1(string checksumQuery, string connectionStr)
        {
            var hashes = new List<int>();

            using (var conn = new SqlConnection(connectionStr))
            using (var cmd = new SqlCommand())
            {
                cmd.Connection  = conn;
                cmd.CommandText = checksumQuery;

                await conn.OpenAsync();
                var readr = await cmd.ExecuteReaderAsync();

                while (readr.Read())
                    hashes.Add(readr.GetInt32(0));

                conn.Close();
            }
            if (hashes.Count == 0) return null;

            return string.Join("", hashes).SHA1();
        }
    }
}
