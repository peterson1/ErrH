﻿using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Threading;
using System.Threading.Tasks;
using ErrH.Tools.Extensions;
using ErrH.Tools.Loggers;
using ErrH.Tools.SqlHelpers;

namespace ErrH.OleDbShim
{
    public class MsSql2008ReadOnly : LogSourceBase, IOleDbClientReadOnly
    {
        private OleDbConnection _conn;


        public bool IsConnected => _conn?.State == ConnectionState.Open 
                                || _conn?.State == ConnectionState.Fetching
                                || _conn?.State == ConnectionState.Executing;


        public async Task<bool> Connect(string serverUrlOrFilePath, string databaseName, string userName, string password, CancellationToken token)
        {
            Debug_n($"Connecting to DB “{databaseName}”...", $"URL: ‹ {serverUrlOrFilePath} ›");

            var cnStr = $"Provider=SQLOLEDB;"
                      + $"Data Source={serverUrlOrFilePath};"
                      + $"Initial Catalog={databaseName};"
                      + $"User ID={userName};"
                      + $"Password={password};";

            return await Connect(cnStr, token).ConfigureAwait(false);
        }


        public async Task<bool> Connect(string connectionString, CancellationToken token)
        {
            _conn = new OleDbConnection(connectionString);
            try
            {
                await _conn.OpenAsync(token).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return Error_(false, "Failed to connect to database.", L.f + ex.Details(false, false));
            }
            return Debug_(true, "Successfully connected to database.", "");
        }



        public async Task<object> Scalar(string sqlQuery, CancellationToken token)
        {
            var cmd = NewCommand(sqlQuery);
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
            var cmd = NewCommand(sqlQuery);
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

            Debug_n("Successfully executed SQL query.", $"readr.RecordsAffected : {readr.RecordsAffected}");
            return await Shimify(readr);
        }


        private async Task<RecordSetShim> Shimify(DbDataReader readr)
        {
            var shim = new RecordSetShim();
            var colCount = readr.FieldCount;

            while (await readr.ReadAsync())
            {
                var row = new ResultRow();

                for (int j = 0; j < colCount; j++)
                    row.Add(await readr.GetFieldValueAsync<object>(j));

                shim.Add(row);
            }
            return shim;
        }


        private OleDbCommand NewCommand(string sqlQuery)
        {
            if (!CanExecute()) return null;

            var cmd = _conn.CreateCommand();
            cmd.CommandText = sqlQuery;
            Debug_n("Fetching data using SQL query...", sqlQuery);
            return cmd;
        }


        private bool CanExecute()
        {
            if (_conn == null)
                return Warn_n("Connection instance is NULL.", "Connect() may have failed or have not been called.");

            if (_conn.State != ConnectionState.Open)
                return Warn_n("Connection state is not ‹Open›.", $"_conn.State = ‹{_conn.State}›");

            return true;
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
                    try {  _conn?.Close();
                           _conn?.Dispose();  }
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