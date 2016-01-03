using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ErrH.Tools.Drupal7Models;
using ErrH.Tools.Drupal7Models.Entities;
using ErrH.Tools.Extensions;
using ErrH.Tools.Loggers;
using ErrH.Tools.Serialization;
using ErrH.Tools.SqlHelpers;

namespace ErrH.Drupal7Client.Derivatives
{
    public abstract class D7Writer2Base<T> : LogSourceBase, ID7Writer2<T>
        where T : class, ID7Node, new()
    {
        private int                _addedCount;
        private int                _editedCount;
        private int                _deletedCount;
        private ISerializer        _serialr;
        private Dictionary<int, T> _dict          = new Dictionary<int, T>();
        private List<T>            _toAddNodes    = new List<T>();
        private HashSet<T>         _toEditNodes   = new HashSet<T>();
        private HashSet<T>         _toDeleteNodes = new HashSet<T>();


        public ID7Client     Client    { get; set; }
        public ISqlDbReader  SqlReader { get; set; }
        public string        TaskTitle { get; set; }


        public T     Tracked     (int nid) => _dict[nid];
        public void  AddLater    (T node)  => _toAddNodes.Add(node);
        public void  DeleteLater (T node)  => _toDeleteNodes.Add(node);
        public bool  WillAdd     (Func<T, bool> filter) => _toAddNodes.Has(filter);
        public bool  WillEdit    (Func<T, bool> filter) => _toEditNodes.Has(filter);
        public bool  WillDelete  (Func<T, bool> filter) => _toDeleteNodes.Has(filter);

        public bool  HasChanges  => _toAddNodes.Count() 
                                  + _toEditNodes.Count() 
                                  + _toDeleteNodes.Count() > 0;

        public IEnumerable<T> Tracked(Func<T, bool> filter)
            => _dict.Values.Where(filter).ToList();


        public D7Writer2Base(ISerializer serializer)
        {
            _serialr = serializer;
        }



        public IEnumerable<T> TrackChanges(IEnumerable<T> nodes)
        {
            ClearLists();
            var list = nodes.ToList();
            foreach (var node in list)
            {
                node.PropertyChanged += (s, e) =>
                    _toEditNodes.Add(node);

                _dict.Add(node.nid, node);
            }
            return list;
        }


        public async Task<bool> SaveChanges(CancellationToken tkn = default(CancellationToken))
        {
            foreach (var node in _toAddNodes)
                if (!await Try(AddNow, node, tkn)) return false;


            foreach (var node in _toEditNodes)
            {
                if (!await Try(EditNow, node, tkn)) return false;
                //TaskTitle = node.ToString();
                //RaiseProgressInfo();
            }


            foreach (var node in _toDeleteNodes)
                if (!await Try(DeleteNow, node, tkn)) return false;

            return ClearLists();
        }


        private async Task<bool> Try(Func<T, CancellationToken, Task<bool>> method, T node, CancellationToken tkn)
        {
            try {
                return await method.Invoke(node, tkn);
            }
            catch (Exception ex)
            {
                Error_n("Failed to save node.", _serialr.Write(node, true));
                return LogError("SaveChanges", ex);
            }
        }


        private async Task<bool> AddNow(T node, CancellationToken tkn)
        {
            if (!CanExecute(node)) return false;

            D7NodeBase dto;
            try { dto = D7FieldMapper.Map(node); }
            catch (Exception ex) { return LogError("D7FieldMapper.Map", ex); }

            if (dto == null) return Error_n("Failed to map to D7 fields.", "");

            D7NodeBase saved;
            try { saved = await Client.Post(dto, tkn); }
            catch (Exception ex) { return LogError("_client.Post", ex); }

            if (saved == null || saved.nid < 1)
                return Error_n("Failed to add item to repo.", "");

            _addedCount += 1;
            return RaiseProgressInfo();
        }


        private async Task<bool> EditNow(T node, CancellationToken tkn)
        {
            if (!CanExecute(node)) return false;

            ID7NodeRevision dto;
            try { dto = D7FieldMapper.Map(node).As<ID7NodeRevision>(); }
            catch (Exception ex)
            { return LogError("D7FieldMapper.Map(item)", ex); }

            if (dto == null) return false;
            dto.nid = node.nid;
            dto.vid = node.As<ID7NodeRevision>().vid;
            if (!await Client.Put(dto, tkn)) return false;

            _editedCount += 1;
            return RaiseProgressInfo();
        }


        private async Task<bool> DeleteNow(T node, CancellationToken tkn)
        {
            if (!CanExecute(node)) return false;
            if (!await Client.Delete(node.nid, tkn)) return false;
            _deletedCount += 1;
            return RaiseProgressInfo();
        }


        private bool CanExecute(T node)
        {
            if (node == null)
                return Error_n($"{WName} cannot execute", $"{TName} == null");

            if (Client == null)
                return Error_n($"{WName} cannot execute", $"‹{typeof(ID7Client).Name}› == null");

            if (!Client.IsLoggedIn)
                return Error_n($"{WName} cannot execute", $"‹{CName}› is not logged in.");

            return true;
        }

        private string TName => typeof(T).Name;
        private string CName => Client?.GetType().Name;
        private string WName => this.GetType().Name;


        private bool RaiseProgressInfo()
        {
            var m = $"add {_addedCount}/{_toAddNodes.Count}" 
              + $" : edit {_editedCount}/{_toEditNodes.Count}" 
              + $" : delete {_deletedCount}/{_toDeleteNodes.Count}";

            return Info_n(TaskTitle, m);
        }


        private bool ClearLists()
        {
            _addedCount = _editedCount = _deletedCount = 0;

            _dict.Clear();
            _toAddNodes.Clear();
            _toEditNodes.Clear();
            _toDeleteNodes.Clear();

            return true;
        }



        public virtual void Dispose()
        {
            ClearLists();
            Client = null;
        }
    }
}
