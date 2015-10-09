using System;
using System.Threading;
using System.Threading.Tasks;
using ErrH.Tools.Loggers;
using ErrH.Tools.SqlHelpers;

namespace ErrH.SqlClientShim
{
    public class SqlDbClient : LogSourceBase, ISqlClient
    {
        private ISqlExecutor _exec;


        public bool IsConnected { get; private set; }



        public SqlDbClient(ISqlExecutor sqlExecutor)
        {
            _exec = ForwardLogs(sqlExecutor);
        }


        public async Task<bool> Connect(string serverUrlOrFilePath,
                                        string databaseName,
                                        string userName,
                                        string password,
                                        CancellationToken token)
        {
            if (IsConnected) return true;

            try {
                return IsConnected = await TaskEx.Run(() 
                    => _exec.Connect(serverUrlOrFilePath, 
                        databaseName, userName, password, token));
            }
            catch (Exception ex)
            {  return LogError("await _exec.Connect", ex); }
        }


        public async Task<int> ExecuteNonQuery(string sqlCommand,
                                               CancellationToken token)
        {
            try {
                return await TaskEx.Run(() 
                    => _exec.ExecuteNonQuery(sqlCommand, token));
            }
            catch (Exception ex)
            {
                LogError("await _exec.ExecuteNonQuery", ex);
                return -1;
            }
        }



        public void Dispose()
        {
            if (_exec != null) _exec.Dispose();
            IsConnected = false;
        }

    }
}
