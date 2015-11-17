using System;
using System.Threading;
using System.Threading.Tasks;
using ErrH.Tools.Loggers;
using ErrH.Tools.SqlHelpers;

namespace ErrH.SqlClientShim
{
    public class DbReaderPclShim : LogSourceBase, ISqlDbReader
    {
        private IDbReaderNative _oleDb;

        public bool IsConnected => _oleDb.IsConnected;


        public DbReaderPclShim(IDbReaderNative oleDbClient)
        {
            _oleDb = ForwardLogs(oleDbClient);
        }



        public async Task<bool> Connect(string connectionString, CancellationToken token = default(CancellationToken))
        {
            if (IsConnected) return true;
            try {
                return await TaskEx.Run(()
                    => _oleDb.Connect(connectionString, token));
            }
            catch (Exception ex)
            { return LogError("await _oleDb.Connect", ex); }
        }


        public async Task<bool> Connect(string serverUrlOrFilePath, string databaseName, string userName, string password, CancellationToken token)
        {
            if (IsConnected) return true;
            try {
                return await TaskEx.Run(()
                    => _oleDb.Connect(serverUrlOrFilePath,
                        databaseName, userName, password, token));
            }
            catch (Exception ex)
            { return LogError("await _oleDb.Connect", ex); }
        }




        public async Task<object> Scalar(string sqlQuery, CancellationToken token)
        {
            try {
                return await TaskEx.Run(()
                    => _oleDb.Scalar(sqlQuery, token));
            }
            catch (Exception ex)
            {
                LogError("await _oleDb.Scalar", ex);
                return null;
            }
        }


        public async Task<int> RecordCount(string tableName, string whereClause = "", CancellationToken token = default(CancellationToken))
        {
            try {
                return await TaskEx.Run(()
                    => _oleDb.RecordCount(tableName, whereClause, token));
            }
            catch (Exception ex)
            {
                LogError("await _oleDb.RecordCount", ex);
                return -1;
            }
        }


        public async Task<RecordSetShim> Query(string sqlQuery, CancellationToken token = default(CancellationToken))
        {
            try {
                return await TaskEx.Run(()
                    => _oleDb.Query(sqlQuery, token));
            }
            catch (Exception ex)
            {
                LogError("await _oleDb.Query", ex);
                return null;
            }
        }



        public async Task<ResultRow> Get1(string sqlQuery, CancellationToken token = default(CancellationToken))
        {
            try {
                return await TaskEx.Run(()
                    => _oleDb.Get1(sqlQuery, token));
            }
            catch (Exception ex)
            {
                LogError("await _oleDb.Get1", ex);
                return null;
            }
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
                    _oleDb?.Dispose();
                    _oleDb = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~SqlReadOnlyClient() {
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
