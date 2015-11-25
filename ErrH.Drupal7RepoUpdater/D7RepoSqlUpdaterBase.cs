using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ErrH.Drupal7RepoUpdater.DTOs;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.Drupal7Models;
using ErrH.Tools.Drupal7Models.Entities;
using ErrH.Tools.Drupal7Models.FieldAttributes;
using ErrH.Tools.ErrorConstructors;
using ErrH.Tools.Extensions;
using ErrH.Tools.Loggers;
using ErrH.Tools.Serialization;
using ErrH.Tools.SqlHelpers;
using PropertyChanged;

namespace ErrH.Drupal7RepoUpdater
{
    [ImplementPropertyChanged]
    public abstract class D7RepoSqlUpdaterBase<T>
        : LogSourceBase, IRepoUpdater<T>
        where T : D7NodeBase, new()
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private   ISerializer   _serialr;
        protected IMapOverride  _mapOverride;



        public D7RepoSqlUpdaterBase(ISerializer serializer)
        {
            _serialr   = serializer;
            JobTitle   = $"‹{typeof(T).Name}› Repo Updater";
            JobMessage = "Idle.";
        }


        public ISqlDbReader DbReader       { get; set; }
        public string       JobTitle       { get; protected set; }
        public string       JobMessage     { get; protected set; }
        public int          ProgressValue  { get; protected set; }
        public int          ProgressTotal  { get; protected set; }


        public abstract string   ResourceURL  { get; }


        public virtual string GetSqlQuery(params object[] args)
            => SqlBuilder.SELECT<T>();


        public virtual async Task<bool> Update(IRepository<T> repo, 
                                               CancellationToken token = new CancellationToken(),
                                               params object[] args)
        {
            InitializeProgressState(repo);

            var sqlTask = DbReader.Query(GetSqlQuery(args), token);
            var repoTask = QueryTargetD7(repo, ResourceURL, token, args);

            try { await TaskEx.WhenAll(sqlTask, repoTask); }
            catch (Exception ex)
            { return Error_n("Error on Update()", ex.Details(false, false)); }

            var sqlResult = await sqlTask;
            var repoResult = await repoTask;

            if (sqlResult == null)
                return Error_n("SQL query task returned NULL.", "");

            if (repoResult == null)
                return Error_n("D7 query task returned NULL.", ResourceURL);

            var sC = sqlResult.Count;
            var rC = repoResult.Count();

            Info_n("Total records returned:",
                 $"{sC} in SQL DB; {rC} in D7 server (diff: {sC - rC})");

            Throw.If(rC > sC, "Redundant records in D7 repo.");

            if (!ApplyChangesToRepo(sqlResult,
                repoResult, repo, _mapOverride))
                return false;

            Info_n("Saving changes to repo...",
                    $"new nodes: {repo.NewUnsavedItems.Count} ;"
                  + $" modified: {repo.ChangedUnsavedItems.Count}");

            ProgressTotal = repo.NewUnsavedItems.Count
                          + repo.ChangedUnsavedItems.Count;

            return await repo.SaveChangesAsync(token);
        }


        private void InitializeProgressState(IRepository<T> repo)
        {
            JobMessage = "Querying SQL and D7 data sources...";
            ProgressTotal = 0;
            ProgressValue = 0;
            ((ID7Client)repo.Client).ResponseReceived
                += (s, e) => ProgressValue++;
        }


        private async Task<IEnumerable<NodeRecordHash>> QueryTargetD7
            (IRepository<T> repo, string resourceURL, CancellationToken token, object[] args)
        {
            var client = repo.Client as ID7Client;
            var d7Typ  = D7NodeDtoAttribute.Of<T>().MachineName;
            var rsrc   = resourceURL.Slash(d7Typ);

            if (args.Length > 0)
            {
                var joind = string.Join("/", args);
                JobTitle += " : " + joind;
                rsrc = rsrc.Slash(joind);
            }
            return await client.Get<List<NodeRecordHash>>(rsrc, token);
        }
        


        private bool ApplyChangesToRepo(RecordSetShim sqlResult, 
                                        IEnumerable<NodeRecordHash> nodeRecHashes, 
                                        IRepository<T> repo,
                                        IMapOverride overrider)
        {
            Info_n("Applying changes to repo...", "");

            var tblKey  = DbColAttribute.Key<T>()?.Property?.Name;
            if (tblKey.IsBlank())
                return Error_n($"DbCol (IsKey=true) attribute missing from ‹{typeof(T).Name}›", "");

            var hashField = D7HashFieldAttribute.FindIn<T>();

            foreach (var row in sqlResult)
            {
                var dbRecID = row.AsInt(tblKey);
                var dbRowSha1 = _serialr.SHA1(row);

                var repoNode = new T();
                var d7RecHash = nodeRecHashes.FirstOrDefault(x => x.dbID == dbRecID);


                if (d7RecHash != null)
                    repoNode = repo.ByNid(d7RecHash.nid);

                if (dbRowSha1 != d7RecHash?.sha1)
                {
                    DbRowMapper.Map(row, repoNode, overrider);

                    hashField?.ModelProperty?
                        .SetValue(repoNode, dbRowSha1, null);
                }

                if (d7RecHash == null) repo.Add(repoNode);
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
