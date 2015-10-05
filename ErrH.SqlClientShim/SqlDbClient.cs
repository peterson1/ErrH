using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            return await _exec.Connect(serverUrlOrFilePath, 
                    databaseName, userName, password, token);
        }

    }
}
