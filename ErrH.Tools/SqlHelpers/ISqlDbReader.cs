using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using ErrH.Tools.Loggers;

namespace ErrH.Tools.SqlHelpers
{
    public interface ISqlDbReader : ILogSource, IDisposable
    {
        //event EventHandler ConnStringNeeded;

        bool              IsConnected       { get; }
        bool              IsBusy            { get; }
        string            ConnectionString  { get; set; }
        object            DbConnection      { get; set; }
        SqlServerKeyFile  KeyFile           { get; set; }

        Task<bool> Connect (string serverUrlOrFilePath,
                            string databaseName,
                            string userName,
                            string password,
                            CancellationToken token = new CancellationToken());

        Task<bool> Connect(string connectionString,
                           CancellationToken token = new CancellationToken());

        Task<bool> Connect(CancellationToken token = new CancellationToken());


        Task<object> Scalar (string sqlQuery,
                             CancellationToken token = new CancellationToken());

        Task<int> RecordCount (string tableName, 
                               string whereClause = "",
                               CancellationToken token = new CancellationToken());

        Task<RecordSetShim> Query (string sqlQuery,
                                   CancellationToken token = new CancellationToken());

        Task<ResultRow> Get1 (string sqlQuery,
                              CancellationToken token = new CancellationToken());

        Task<IDictionary<TKey, TVal>> QueryDictionary<TKey, TVal>(string twoColumnQuery,
                              CancellationToken token = new CancellationToken());
    }
}
