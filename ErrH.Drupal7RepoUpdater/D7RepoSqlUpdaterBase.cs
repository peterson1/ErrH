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
        where T : ID7Node, new()
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected ISerializer   _serialr;
        protected IMapOverride  _mapOverride;


        public D7RepoSqlUpdaterBase(ISerializer serializer)
        {
            _serialr   = serializer;
        }


        public ISqlDbReader DbReader       { get; set; }
        public string       JobTitle       { get; protected set; }
        public string       JobMessage     { get; protected set; }
        public int          ProgressValue  { get; protected set; }
        public int          ProgressTotal  { get; protected set; }


        protected abstract ID7Writer<T> _writr              { get; }
        protected abstract string       _resourceURL        { get; }
        protected virtual bool          _deleteIfNotInSqlDB { get; } = false;


        public virtual string GetSqlQuery(params object[] args)
            => SqlBuilder.SELECT<T>();


        public virtual async Task<bool> Update(CancellationToken token = new CancellationToken(),
                                               params object[] args)
        {
            InitializeProgressState();
            var qry = GetSqlQuery(args);
            var sqlTask = DbReader.Query(qry, token);
            var repoTask = QueryTargetD7(_resourceURL, token, args);

            try { await TaskEx.WhenAll(sqlTask, repoTask); }
            catch (Exception ex)
            { return Error_n("Error on Update()", ex.Details(false, false)); }

            var sqlResult = await sqlTask;
            var repoResult = await repoTask;

            if (sqlResult == null)
                return Error_n("SQL query task returned NULL.", qry);

            if (repoResult == null)
                return Error_n("D7 query task returned NULL.", _resourceURL);

            var sC = sqlResult.Count;
            var rC = repoResult.Count();

            Info_n($"Current ‹{typeof(T).Name}› records:",
                 $"{sC} in SQL DB; {rC} in D7 server (diff: {sC - rC})");

            //Throw.If(rC > sC, "Redundant records in D7 repo.");

            if (!ApplyChangesToRepo(sqlResult, 
                repoResult, _mapOverride)) return false;

            if (_writr.NewUnsavedItems.Count 
              + _writr.ChangedUnsavedItems.Count
              + _writr.ToBeDeletedItems.Count == 0)
                return Info_n($"All ‹{typeof(T).Name}› records match.", "Nothing needs saving.");

            Info_n("Saving changes to repo...",
                    $"new nodes: {_writr.NewUnsavedItems.Count} ;"
                  + $" modified: {_writr.ChangedUnsavedItems.Count}"
                  +  $" deleted: {_writr.ToBeDeletedItems.Count}"
                  );

            ProgressTotal = _writr.NewUnsavedItems.Count
                          + _writr.ChangedUnsavedItems.Count
                          + _writr.ToBeDeletedItems.Count;

            //return false;
            return await _writr.SaveChanges(token);
        }


        private void InitializeProgressState()
        {
            JobTitle   = typeof(T).Name;
            JobMessage = "Query ... ";
            ProgressTotal = 0;
            ProgressValue = 0;
            _writr.OneChangeCommitted += (s, e) => ProgressValue++;
        }


        private async Task<IEnumerable<NodeRecordHash>> QueryTargetD7
            (string resourceURL, CancellationToken token, object[] args)
        {
            if (_writr == null) throw Error.NullRef(nameof(_writr));
            if (_writr.Client == null) throw Error.NullRef("_writr.Client");
            var client = _writr.Client;
            var d7Typ  = D7NodeDtoAttribute.Of<T>().MachineName;
            var rsrc   = resourceURL.Slash(d7Typ);

            if (args.Length > 0)
            {
                var joind = JobMessage = string.Join("/", args);
                rsrc = rsrc.Slash(joind);
            }
            return await client.Get<List<NodeRecordHash>>(rsrc, token);
        }
        


        private bool ApplyChangesToRepo(RecordSetShim sqlResult, 
                                        IEnumerable<NodeRecordHash> nodeRecHashes, 
                                        IMapOverride overrider)
        {
            //Info_n("Applying changes to repo...", "");

            var tblKey  = DbColAttribute.Key<T>()?.Property?.Name;
            if (tblKey.IsBlank())
                return Error_n($"DbCol (IsKey=true) attribute missing from ‹{typeof(T).Name}›", "");

            if (_deleteIfNotInSqlDB)
                foreach (var d7n in nodeRecHashes)
                    if (!FoundIn(sqlResult, tblKey, d7n))
                        _writr.DeleteLater(_writr[d7n.nid]);

            var hashField = D7HashFieldAttribute.FindIn<T>();
            foreach (var row in sqlResult)
            {
                var tweakdRow = TweakSqlRow(row);
                var dbRecID   = tweakdRow.AsInt(tblKey);
                var dbRowSha1 = _serialr.SHA1(tweakdRow);
                var repoNode  = new T();
                var d7RecHash = nodeRecHashes.FirstOrDefault(x => x.dbID == dbRecID);


                if (d7RecHash != null)
                {
                    repoNode = _writr[d7RecHash.nid];
                    if (repoNode == null)
                        return Error_n("Nid found in hash-json BUT NOT in repo-json.", "You may need to clear/reload the repo.");
                }

                if (dbRowSha1 != d7RecHash?.sha1)
                {
                    if (!MapValues(overrider, tweakdRow, repoNode)) return false;

                    hashField?.ModelProperty?
                        .SetValue(repoNode, dbRowSha1, null);
                }

                if (d7RecHash == null) _writr.AddLater(repoNode);
            }
            return true;
        }


        private bool FoundIn(RecordSetShim sqlResult, string tblKey, NodeRecordHash d7n)
        {
            foreach (var row in sqlResult)
                if (row[tblKey].ToInt() == d7n.dbID) return true;

            return false;
        }


        private bool MapValues(IMapOverride overrider, ResultRow row, T repoNode)
        {
            try {
                return DbRowMapper.Map(row, repoNode, overrider);
            }
            catch (Exception ex) {
                return LogError("DbRowMapper.Map", ex);
            }
            //Debug_n("DbRowMapper.Map success", "");
        }


        public virtual ResultRow TweakSqlRow(ResultRow row)
        {
            return row;
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
