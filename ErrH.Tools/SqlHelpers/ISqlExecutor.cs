using System;
using System.Threading;
using System.Threading.Tasks;
using ErrH.Tools.Loggers;
using ErrH.Tools.ScalarEventArgs;

namespace ErrH.Tools.SqlHelpers
{
    public interface ISqlExecutor : ILogSource, IDisposable
    {
        Task<bool> Connect(string serverUrlOrFilePath,
                           string databaseName,
                           string userName,
                           string password,
                           CancellationToken token = new CancellationToken());

        Task<bool> Connect(string connectionString, 
                           CancellationToken token = new CancellationToken());

        Task<int> ExecuteNonQuery(string sqlCommand, 
                                  CancellationToken token = new CancellationToken());

    }
}
