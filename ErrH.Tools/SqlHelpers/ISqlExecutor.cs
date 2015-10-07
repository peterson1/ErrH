using System;
using System.Threading;
using System.Threading.Tasks;
using ErrH.Tools.Loggers;
using ErrH.Tools.ScalarEventArgs;

namespace ErrH.Tools.SqlHelpers
{
    public interface ISqlExecutor : ILogSource, IDisposable
    {
        //event EventHandler<EArg<int>> ExecuteNonQueryReturned;

        Task<bool> Connect(string serverUrlOrFilePath,
                           string databaseName,
                           string userName,
                           string password,
                           CancellationToken token = new CancellationToken());

        Task<bool> Connect(string connectionString, 
                           CancellationToken token = new CancellationToken());

        Task<int> ExecuteNonQuery(string sqlCommand, 
                                  CancellationToken token = new CancellationToken());

        //int ExecuteNonQuery(string sqlCommand);

        //void ExecuteNonQuery(string serverUrlOrFilePath,
        //                     string databaseName,
        //                     string userName,
        //                     string password,
        //                     string sqlCommand);
    }
}
