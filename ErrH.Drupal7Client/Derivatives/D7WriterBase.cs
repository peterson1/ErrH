using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ErrH.Tools.Drupal7Models;
using ErrH.Tools.Drupal7Models.Entities;
using ErrH.Tools.ErrorConstructors;
using ErrH.Tools.Extensions;
using ErrH.Tools.Loggers;
using ErrH.Tools.Serialization;
using PropertyChanged;

namespace ErrH.Drupal7Client.Derivatives
{
    [ImplementPropertyChanged]
    public abstract class D7WriterBase<T> : LogSourceBase, ID7Writer<T>
        where T : class, ID7Node, new()
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private      EventHandler _oneChangeCommitted;
        public event EventHandler  OneChangeCommitted
        {
            add    { _oneChangeCommitted -= value; _oneChangeCommitted += value; }
            remove { _oneChangeCommitted -= value; }
        }


        private ISerializer _serialr;

        protected Dictionary<int, T> _dict                = new Dictionary<int, T>();
        protected List<T>            _newUnsavedItems     = new List<T>();
        protected HashSet<T>         _changedUnsavedItems = new HashSet<T>();
        protected HashSet<T>         _toBeDeletedItems    = new HashSet<T>();


        public ID7Client             Client              { get; set; }
        public ReadOnlyCollection<T> NodesTracked        => new ReadOnlyCollection<T>(_dict.Values.ToList());
        public ReadOnlyCollection<T> NewUnsavedItems     => new ReadOnlyCollection<T>(_newUnsavedItems);
        public ReadOnlyCollection<T> ChangedUnsavedItems => new ReadOnlyCollection<T>(_changedUnsavedItems.ToList());
        public ReadOnlyCollection<T> ToBeDeletedItems    => new ReadOnlyCollection<T>(_toBeDeletedItems.ToList());

        public T      this         [int nid]  => _dict[nid];
        public void   AddLater    (T newNode) => _newUnsavedItems.Add(newNode);
        public void   DeleteLater (T newNode) => _toBeDeletedItems.Add(newNode);
        public string ChangeSummary           => GetSummary();

        public string  JobTitle      { get; set; }
        public string  JobMessage    { get; set; }
        public int     ProgressTotal { get; set; }
        public int     ProgressValue { get; set; }


        public D7WriterBase(ISerializer serializer)
        {
            _serialr = serializer;
        }


        public IEnumerable<T> TrackChanges(IEnumerable<T> nodes)
        {
            _dict.Clear();
            _newUnsavedItems.Clear();
            _changedUnsavedItems.Clear();

            foreach (var node in nodes)
            {
                node.PropertyChanged += (s, e) =>
                    _changedUnsavedItems.Add(node);

                _dict.Add(node.nid, node);
            }

            return nodes;
        }





        public virtual async Task<bool> SaveChanges(CancellationToken tkn = default(CancellationToken))
        {
            InitializeProgressState();
            bool ok = false;
            OneChangeCommitted += (s, e) => ProgressValue++;

            foreach (var item in _newUnsavedItems)
            {
                try { ok = await AddItem(item, tkn); }
                catch (Exception ex)
                { return LogError($"AddItem: new ‹{typeof(T).Name}› “{item.title}”", ex); }
                if (!ok) return Warn_n("Failed to POST new node: ", _serialr.Write(item, true));
            }
            _newUnsavedItems.Clear();


            foreach (var item in _changedUnsavedItems)
            {
                try { if (!await UpdateItem(item, tkn)) return false; }
                catch (Exception ex)
                { return LogError($"UpdateItem: [nid:{item?.nid}] «{item?.title}»", ex); }
            }
            _changedUnsavedItems.Clear();


            foreach (var item in _toBeDeletedItems)
            {
                try { if (!await DeleteItem(item, tkn)) return false; }
                catch (Exception ex)
                { return LogError($"DeleteItem: [nid:{item?.nid}] «{item?.title}»", ex); }
            }
            _toBeDeletedItems.Clear();

            return true;
        }


        private async Task<bool> AddItem(T item, CancellationToken tkn)
        {
            D7NodeBase dto;
            try { dto = D7FieldMapper.Map(item); }
            catch (Exception ex) { return LogError("D7FieldMapper.Map", ex); }

            if (dto == null) return Error_n("Failed to map to D7 fields.", "");

            D7NodeBase node;
            try { node = await Client.Post(dto, tkn); }
            catch (Exception ex) { return LogError("_client.Post", ex); }

            if (node == null || node.nid < 1)
                return Error_n("Failed to add item to repo.", "");

            RaiseOneChangeCommitted();
            return true;
        }


        private async Task<bool> UpdateItem(T item, CancellationToken tkn)
        {
            Throw.IfNull(item, "node to update");
            Throw.IfNull(Client, "‹ID7Client›_client instance");

            ID7NodeRevision dto;
            try { dto = D7FieldMapper.Map(item).As<ID7NodeRevision>(); }
            catch (Exception ex)
            { return LogError("D7FieldMapper.Map(item)", ex); }

            if (dto == null) return false;
            dto.nid = item.nid;
            dto.vid = item.As<ID7NodeRevision>().vid;
            if (!await Client.Put(dto, tkn)) return false;
            RaiseOneChangeCommitted();
            return true;
        }


        private string GetSummary()
        {
            return "Changes"
                + $": new: {_newUnsavedItems.Count}"
                + $"; modified: {_changedUnsavedItems.Count}"
                + $"; deleted: {_toBeDeletedItems.Count}"
                ;
        }


        protected void InitializeProgressState()
        {
            JobTitle      = typeof(T).Name;
            ProgressTotal = _newUnsavedItems.Count 
                          + _changedUnsavedItems.Count
                          + _toBeDeletedItems.Count;
            ProgressValue = 0;
        }


        private Task<bool> DeleteItem(T item, CancellationToken tkn)
        {
            return Client.Delete(item.nid, tkn);
        }


        private void RaiseOneChangeCommitted()
            => _oneChangeCommitted?.Invoke(this, EventArgs.Empty);


    }
}
