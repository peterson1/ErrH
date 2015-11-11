using System;
using System.Threading;
using System.Threading.Tasks;
using ErrH.Tools.Loggers;

namespace ErrH.Tools.SqlHelpers
{
    public interface ISqlClient : ILogSource, IDisposable
    {
        bool IsConnected { get; }

        Task<bool> Connect(string serverUrlOrFilePath,
                           string databaseName,
                           string userName,
                           string password,
                           CancellationToken token);

        Task<int> ExecuteNonQuery(string sqlCommand,
                                  CancellationToken token);

    }
}
