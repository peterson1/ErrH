using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrH.Tools.Extensions;
using ErrH.Tools.SqlHelpers;

namespace ErrH.WpfRestClient.net45.DataAccess
{
    public partial class SQL
    {
        public static SqlClientBase Client (string connectionStr)
        {
            return connectionStr.Contains("\\")
                ? SQL.Lite(connectionStr)
                : SQL.Server(connectionStr);

        }
    }



    public abstract class SqlClientBase
    {
        protected abstract DbConnection  CreateConnection (string connectionStr);
        protected abstract DbCommand     CreateCommand    (string sqlQuery);

        protected string _connStr;


        public SqlClientBase(string connectionStr)
        {
            _connStr = connectionStr;
        }


        public async Task<string> ChecksumSHA1
            (string checksumQuery)
        {
            var hashes = new List<int>();

            await FillList(hashes, _ => _.GetInt32(0),
                checksumQuery);

            if (hashes.Count == 0) return null;
            return string.Join("", hashes).SHA1();
        }




        public async Task<RecordSetShim> QueryRS
            (string sqlQuery)
        {
            var rs = new RecordSetShim();

            await FillList(rs, r =>
            {
                var row = new ResultRow();

                for (int i = 0; i < r.FieldCount; i++)
                    row.Add(r.GetName(i), r[i]);

                return row;
            },
            sqlQuery);
            return rs;
        }



        private async Task FillList<T>
            (List<T> list, Func<DbDataReader, T> function,
            string sqlQuery)
        {
            using (var conn = CreateConnection(_connStr))
            using (var cmd = CreateCommand(sqlQuery))
            {
                cmd.Connection = conn;
                cmd.CommandText = sqlQuery;

                await conn.OpenAsync();
                var readr = await cmd.ExecuteReaderAsync();

                while (readr.Read())
                    list.Add(function(readr));

                conn.Close();
            }
        }

    }
}
