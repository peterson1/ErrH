using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ErrH.Tools.Loggers;
using ErrH.Tools.SqlHelpers;

namespace ErrH.SqlClientShim
{
    public class DbReaderPclShim : LogSourceBase, ISqlDbReader
    {
        private IDbReaderNative _nativeReadr;

        public bool  IsConnected   => _nativeReadr.IsConnected;
        public bool  IsBusy        => _nativeReadr.IsBusy;


        public string ConnectionString
        {
            get { return _nativeReadr.ConnectionString;  }
            set { _nativeReadr.ConnectionString = value; }
        }

        public object DbConnection
        {
            get { return _nativeReadr.DbConnection; }
            set { _nativeReadr.DbConnection = value; }
        }

        public SqlServerKeyFile KeyFile
        {
            get { return _nativeReadr.KeyFile;  }
            set { _nativeReadr.KeyFile = value; }
        }



        public DbReaderPclShim(IDbReaderNative nativeDbReader)
        {
            _nativeReadr = ForwardLogs(nativeDbReader);
        }



        public async Task<bool> Connect(string serverUrlOrFilePath, string databaseName, string userName, string password, CancellationToken token)
        {
            try {
                return await TaskEx.Run(()
                    => _nativeReadr.Connect(serverUrlOrFilePath,
                        databaseName, userName, password, token));
            }
            catch (Exception ex)
            { return LogError("await _oleDb.Connect", ex); }
        }


        public async Task<bool> Connect(string connectionString, CancellationToken token = default(CancellationToken))
        {
            try {
                return await TaskEx.Run(()
                    => _nativeReadr.Connect(connectionString, token));
            }
            catch (Exception ex)
            { return LogError("await _oleDb.Connect", ex); }
        }


        public async Task<bool> Connect(CancellationToken token = default(CancellationToken))
        {
            try {
                return await TaskEx.Run(()
                    => _nativeReadr.Connect(token));
            }
            catch (Exception ex)
            { return LogError("await _oleDb.Connect", ex); }
        }




        public async Task<object> Scalar(string sqlQuery, CancellationToken token)
        {
            try {
                return await TaskEx.Run(()
                    => _nativeReadr.Scalar(sqlQuery, token));
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
                    => _nativeReadr.RecordCount(tableName, whereClause, token));
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
                    => _nativeReadr.Query(sqlQuery, token));
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
                    => _nativeReadr.Get1(sqlQuery, token));
            }
            catch (Exception ex)
            {
                LogError("await _oleDb.Get1", ex);
                return null;
            }
        }



        public async Task<IDictionary<TKey, TVal>> QueryDictionary<TKey, TVal>
            (string sqlQuery, CancellationToken token = default(CancellationToken))
        {
            try {
                return await TaskEx.Run(()
                    => _nativeReadr.QueryDictionary<TKey, TVal>(sqlQuery, token));
            }
            catch (Exception ex)
            {
                LogError("await _oleDb.QueryDictionary", ex);
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
                    _nativeReadr?.Dispose();
                    _nativeReadr = null;
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
