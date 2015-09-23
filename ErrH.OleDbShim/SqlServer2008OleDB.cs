using System;
using System.Data.OleDb;
using System.Threading.Tasks;
using ErrH.Tools.Extensions;
using ErrH.Tools.Loggers;
using ErrH.Tools.ScalarEventArgs;
using ErrH.Tools.SqlHelpers;

namespace ErrH.OleDbShim
{
    public class SqlServer2008OleDB : LogSourceBase, ISqlExecutor
    {
        public event EventHandler<EArg<int>> ExecuteNonQueryReturned;


        private OleDbConnection _conn;



        public async Task<bool> Connect(string serverUrlOrFilePath, string databaseName = null, string userName = null, string password = null)
        {
            Debug_n($"Connecting to DB “{databaseName}”...", $"URL: ‹ {serverUrlOrFilePath} ›");

            var cnStr = $"Provider=SQLOLEDB;"
                      + $"Data Source={serverUrlOrFilePath};"
                      + $"Initial Catalog={databaseName};"
                      + $"User ID={userName};"
                      + $"Password={password};";

            return await Connect(cnStr);
        }


        public async Task<bool> Connect(string connectionString)
        {
            _conn = new OleDbConnection(connectionString);
            try
            {
                await _conn.OpenAsync();
            }
            catch (Exception ex)
            {
                return Error_(false, "Failed to connect to database.", L.f + ex.Details(false, false));
            }
            //return Debug_(true, "Successfully connected to database.", $"logged in as : “{userName}”");
            return Debug_(true, "Successfully connected to database.", "");
        }


        public async Task<int> ExecuteNonQuery(string sqlCommand)
        {
            var cmd = _conn.CreateCommand();
            cmd.CommandText = sqlCommand;
            Debug_n("Executing non-query SQL command...", sqlCommand);

            int ret = -1; try
            {
                ret = await cmd.ExecuteNonQueryAsync();
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


        private void RaiseExecuteNonQueryReturned(int affectedRows)
            => ExecuteNonQueryReturned?.Invoke(this, new EArg<int> { Value = affectedRows });


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
