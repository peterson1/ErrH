using System;
using System.Threading;
using System.Threading.Tasks;
using ErrH.Tools.Loggers;
using ErrH.Tools.SqlHelpers;

namespace ErrH.SqlClientShim
{
    public class SqlDbClient : LogSourceBase
    {
        private ISqlExecutor _exec;



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
            try
            {
                return await TaskEx.Run(() 
                    => _exec.Connect(serverUrlOrFilePath, 
                        databaseName, userName, password, token));

            }
            catch (Exception ex)
            {
                return LogError("await _exec.Connect", ex);
            }
        }



        public async Task<int> ExecuteNonQuery(string sqlCommand,
                                               CancellationToken token)
        {
            try
            {
                return await TaskEx.Run(() 
                    => _exec.ExecuteNonQuery(sqlCommand, token));
            }
            catch (Exception ex)
            {
                LogError("await _exec.ExecuteNonQuery", ex);
                return -1;
            }
        }


    }
}
