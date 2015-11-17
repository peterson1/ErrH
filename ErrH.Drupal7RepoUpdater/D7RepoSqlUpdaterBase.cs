using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ErrH.Drupal7RepoUpdater.DTOs;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.Drupal7Models;
using ErrH.Tools.Drupal7Models.Entities;
using ErrH.Tools.Extensions;
using ErrH.Tools.Loggers;
using ErrH.Tools.SqlHelpers;

namespace ErrH.Drupal7RepoUpdater
{
    public abstract class D7RepoSqlUpdaterBase<T>
        : LogSourceBase, IRepoUpdater<T>
        where T : D7NodeBase
    {


        public ISqlDbReader DbReader { get; set; }



        public async Task<bool> Update(IRepository<T> repo, string resourceURL, CancellationToken token = new CancellationToken())
        {
            ((ICacheSource)repo)?.ClearCache();

            var sqlTask = QuerySourceDB(token);
            var repoTask = QueryTargetD7(repo, resourceURL, token);

            try { await TaskEx.WhenAll(sqlTask, repoTask); }
            catch (Exception ex)
            { return Error_n("Error on Update()", ex.Details(false, false)); }

            var sqlResult = await sqlTask;
            var repoResult = await repoTask;

            if (sqlResult == null)
                return Error_n("SQL query task returned NULL.", "");

            if (repoResult == null)
                return Error_n("D7 query task returned NULL.", resourceURL);

            if (!ApplyChangesToRepo(sqlResult, repo)) return false;

            Info_n("Saving changes to repo...",
                    $"new nodes: {repo.NewUnsavedItems.Count} ;"
                  + $" modified: {repo.ChangedUnsavedItems.Count}");

            return await repo.SaveChangesAsync(token);
        }


        private async Task<RecordSetShim> QuerySourceDB(CancellationToken token)
        {
            //string svr, db, usr, pwd;
            //if (!SetCredentials(out svr, out db, out usr, out pwd)) return null;

            //Info_n("Connecting to SQL server...", "");
            //if (!await _sql.Connect(svr, db, usr, pwd, token))
            //    return null;

            var qry = SqlBuilder.SELECT<T>();

            return await DbReader.Query(qry, token);
        }


        private async Task<IEnumerable<NodeRecordHash>> QueryTargetD7
            (IRepository<T> repo, string resourceURL, CancellationToken token)
        {
            var client = repo.Client as ID7Client;
            return await client.Get<List<NodeRecordHash>>(resourceURL, token);
        }
        


        private bool ApplyChangesToRepo(RecordSetShim sqlResult, IRepository<T> repo)
        {
            Info_n("Applying changes to repo...", "");

            foreach (var row in sqlResult)
            {
                
            }

            return true;
        }








        #region IDisposable Support

        private bool _isDisposed = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;
            if (disposing)
            {
                // TODO: dispose managed state (managed objects).
                DbReader?.Dispose();
            }

            // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
            // TODO: set large fields to null.

            _isDisposed = true;
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~D7CachedRepoSqlUpdaterBase() {
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
