using System;
using System.Data;
using System.Data.OleDb;
using System.Threading;
using System.Threading.Tasks;
using ErrH.Tools.Extensions;
using ErrH.Tools.Loggers;
using ErrH.Tools.ScalarEventArgs;
using ErrH.Tools.SqlHelpers;

namespace ErrH.OleDbShim
{
    public class SqlServer2008OleDB : LogSourceBase, ISqlExecutor
    {
        //public event EventHandler<EArg<int>> ExecuteNonQueryReturned;


        private OleDbConnection _conn;



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
            //return Debug_(true, "Successfully connected to database.", $"logged in as : “{userName}”");
            return Debug_(true, "Successfully connected to database.", "");
        }


        //later: refactor to extract common logic with async version of method
        //public int ExecuteNonQuery(string sqlCommand)
        //{
        //    if (_conn == null)
        //        return Warn_(-1, "Connection instance is NULL.", "Connect() may have failed or have not been called.");

        //    if (_conn.State != ConnectionState.Open)
        //        return Warn_(-1, "Connection state is not ‹Open›.", $"_conn.State = ‹{_conn.State}›");

        //    var cmd = _conn.CreateCommand();
        //    cmd.CommandText = sqlCommand;
        //    Debug_n("Executing non-query SQL command...", sqlCommand);

        //    int ret = -1; try
        //    {
        //        ret = cmd.ExecuteNonQuery();
        //    }
        //    catch (Exception ex)
        //    {
        //        return Error_(ret, "Error in executing SQL command", ex.Details(false, false));
        //    }

        //    if (ret == 0)
        //        return Warn_(ret, "Successfully executed SQL command BUT ...", $"No rows were affected.");
        //    else
        //        return Debug_(ret, "Successfully executed SQL command.", $"Affected rows: {ret}");
        //}




        public async Task<int> ExecuteNonQuery(string sqlCommand, CancellationToken token)
        {
            if (_conn == null)
                return Warn_(-1, "Connection instance is NULL.", "Connect() may have failed or have not been called.");

            if (_conn.State != ConnectionState.Open)
                return Warn_(-1, "Connection state is not ‹Open›.", $"_conn.State = ‹{_conn.State}›");

            var cmd = _conn.CreateCommand();
            cmd.CommandText = sqlCommand;
            Debug_n("Executing non-query SQL command...", sqlCommand);

            int ret = -1; try
            {
                ret = await cmd.ExecuteNonQueryAsync(token);
            }
            catch (Exception ex)
            {
                return Error_(ret, "Error in executing SQL command", ex.Details(false, false));
            }

            if (ret == 0)
                return Warn_(ret, "Successfully executed SQL command BUT ...", $"No rows were affected.");
            else
                return Debug_(ret, "Successfully executed SQL command.", $"Affected rows: {ret}");
        }


        //public async void ExecuteNonQuery(string serverUrlOrFilePath, 
        //                                  string databaseName, 
        //                                  string userName, 
        //                                  string password, 
        //                                  string sqlCommand)
        //{
        //    var ok = await Connect(serverUrlOrFilePath, databaseName, userName, password);
        //    if (!ok)
        //    {
        //        RaiseExecuteNonQueryReturned(-1);
        //        return;
        //    }
        //    var ret = await ExecuteNonQuery(sqlCommand);
        //    RaiseExecuteNonQueryReturned(ret);
        //}


        //private void RaiseExecuteNonQueryReturned(int affectedRows)
        //    => ExecuteNonQueryReturned?.Invoke(this, new EArg<int> { Value = affectedRows });


        public void Dispose()
        {
            if (_conn == null) return;
            try {
                _conn.Close();
                _conn.Dispose();
            }
            catch { }
        }
    }
}
