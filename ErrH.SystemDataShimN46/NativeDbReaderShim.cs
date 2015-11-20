using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ErrH.Tools.Extensions;
using ErrH.Tools.Loggers;
using ErrH.Tools.SqlHelpers;

namespace ErrH.SystemDataShimN46
{
    public class NativeDbReaderShim : LogSourceBase, IDbReaderNative
    {
        private DbConnection _conn;

        public string            ConnectionString  { get; set; }
        public SqlServerKeyFile  KeyFile           { get; set; }


        public NativeDbReaderShim(DbConnection dbConnection)
        {
            _conn = dbConnection;
        }


        public bool IsConnected => _conn?.State == ConnectionState.Open
                                || _conn?.State == ConnectionState.Fetching
                                || _conn?.State == ConnectionState.Executing;

        public bool IsBusy => _conn?.State == ConnectionState.Fetching
                           || _conn?.State == ConnectionState.Executing;



        public async Task<bool> Connect(string serverUrlOrFilePath, string databaseName, string userName, string password, CancellationToken token)
        {
            Debug_n($"Connecting to DB “{databaseName}”...", $"URL: ‹ {serverUrlOrFilePath} ›");

            var cnStr = ConnStrBuilder.MsSql2008(serverUrlOrFilePath, 
                                                 databaseName, 
                                                 userName, password);

            return await Connect(cnStr, token).ConfigureAwait(false);
        }


        public async Task<bool> Connect(CancellationToken token)
            => await Connect(this.ConnectionString, token);


        public async Task<bool> Connect(string connectionString, CancellationToken token)
        {
            if (IsConnected) return true;
            _conn.ConnectionString = connectionString;
            try
            {
                await _conn.OpenAsync(token).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return Error_(false, "Failed to connect to database.", L.f + ex.Details(false, false));
            }

            this.ConnectionString = connectionString;
            return Debug_(true, "Successfully connected to database.", "");
        }



        public async Task<object> Scalar(string sqlQuery, CancellationToken token)
        {
            var cmd = await NewCommand(sqlQuery, token);
            if (cmd == null) return null;

            object ret = null; try
            {
                ret = await cmd.ExecuteScalarAsync(token);
            }
            catch (Exception ex)
            {
                Error_n("Error in running SQL query", ex.Details(false, false));
            }

            if (ret == null)
                return Warn_(ret, "Successfully executed SQL query BUT ...", $"Object returned is NULL.");
            else
                return Debug_(ret, "Successfully executed SQL query.", $"Returned ‹{ret.GetType().Name}›: {ret.ToString()}");
        }


        public async Task<int> RecordCount(string tableName, string whereClause = "", CancellationToken token = default(CancellationToken))
        {
            var qry = $"SELECT COUNT(1) FROM {tableName}";
            if (!whereClause.IsBlank())
                qry += " WHERE " + whereClause;

            var ret = await Scalar(qry, token);
            return ret?.ToString().ToInt() ?? -1;
        }



        public async Task<RecordSetShim> Query(string sqlQuery, CancellationToken token = default(CancellationToken))
        {
            var cmd = await NewCommand(sqlQuery, token);
            if (cmd == null) return null;

            DbDataReader readr = null; try
            {
                readr = await cmd.ExecuteReaderAsync(token);
            }
            catch (Exception ex)
            {
                Error_n("Error in running SQL query", ex.Details(false, false));
            }

            if (readr == null)
            {
                Warn_n("Successfully executed SQL query BUT ...", $"DbDataReader returned is NULL.");
                return null;
            }

            var shim = await Shimify(readr);
            Debug_n("Successfully executed SQL query.", $"records returned : {shim.Count}");

            return shim;
        }



        public async Task<IDictionary<TKey, TVal>> QueryDictionary<TKey, TVal>
            (string twoColumnQuery, CancellationToken token = default(CancellationToken))
        {
            var ret = new SortedList<TKey, TVal>();
            var rs  = await Query(twoColumnQuery, token);

            foreach (var row in rs)
                ret.Add((TKey)row.ElementAt(0).Value, 
                        (TVal)row.ElementAt(1).Value);
            return ret;
        }



        public async Task<ResultRow> Get1(string sqlQuery, CancellationToken token = default(CancellationToken))
        {
            var rw = (ResultRow)null;
            var rs = await Query(sqlQuery, token);

            if (rs.Count == 0)
                return Warn_(rw, "Query returned ZERO records.", sqlQuery);

            if (rs.Count > 1)
                return Error_(rw, $"Query returned MORE THAN 1 record ({rs.Count}).", sqlQuery);

            return rs[0];
        }



        private async Task<RecordSetShim> Shimify(DbDataReader readr)
        {
            var shim = new RecordSetShim();
            var colCount = readr.FieldCount;

            while (await readr.ReadAsync())
            {
                var row = new ResultRow();

                for (int j = 0; j < colCount; j++)
                {
                    var key = readr.GetName(j);
                    var val = await readr.GetFieldValueAsync<object>(j);
                    row.Add(key, val);
                }

                shim.Add(row);
            }
            return shim;
        }


        private async Task<DbCommand> NewCommand(string sqlQuery, CancellationToken token)
        {
            if (!await CanExecute(token)) return null;

            var cmd = _conn.CreateCommand();
            cmd.CommandText = sqlQuery;
            Debug_n("Fetching data using SQL query...", sqlQuery);
            return cmd;
        }


        private async Task<bool> CanExecute(CancellationToken token)
        {
            if (_conn == null)
                return Warn_n("Connection instance is NULL.", "This should not happen.");

            if (IsBusy)
                return Warn_n("Connection is currently busy.", "Please retry later.");

            if (IsConnected) return true;

            if (!ConnectionString.IsBlank())
                return await Connect(token);

            if (KeyFile != null)
            {
                if (!KeyFile.IsLoaded) KeyFile.ReadFromExeDir();
                return await Connect(KeyFile.ServerURL, KeyFile.DatabaseName, 
                                     KeyFile.UserName, KeyFile.Password, token);
            }

            return Warn_n("No means to connect", "This should never happen."); ;
        }




        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    try
                    {
                        _conn?.Close();
                        _conn?.Dispose();
                    }
                    catch { }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~MsSql2008ReadOnly() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
