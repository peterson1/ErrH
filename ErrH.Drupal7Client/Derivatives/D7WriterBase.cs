using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ErrH.Tools.Drupal7Models;
using ErrH.Tools.Drupal7Models.Entities;
using ErrH.Tools.ErrorConstructors;
using ErrH.Tools.Extensions;
using ErrH.Tools.Loggers;

namespace ErrH.Drupal7Client.Derivatives
{
    public abstract class D7WriterBase<T> : LogSourceBase, ID7Writer<T>
        where T : class, ID7Node, new()
    {
        public event EventHandler OneChangeCommitted;


        private Dictionary<int, T> _dict                = new Dictionary<int, T>();
        private List<T>            _newUnsavedItems     = new List<T>();
        private HashSet<T>         _changedUnsavedItems = new HashSet<T>();


        public ID7Client             Client              { get; set; }
        public ReadOnlyCollection<T> NodesTracked        => new ReadOnlyCollection<T>(_dict.Values.ToList());
        public ReadOnlyCollection<T> NewUnsavedItems     => new ReadOnlyCollection<T>(_newUnsavedItems);
        public ReadOnlyCollection<T> ChangedUnsavedItems => new ReadOnlyCollection<T>(_changedUnsavedItems.ToList());

        public T    this     [int nid]   => _dict[nid];
        public void AddLater (T newNode) => _newUnsavedItems.Add(newNode);



        public void TrackChanges(IEnumerable<T> nodes)
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
        }





        public async Task<bool> SaveChanges(CancellationToken tkn = default(CancellationToken))
        {
            foreach (var item in _newUnsavedItems)
            {
                try { if (!await AddItem(item, tkn)) return false; }
                catch (Exception ex)
                { return LogError($"AddItem: [nid:{item?.nid}] «{item?.title}»", ex); }
            }
            _newUnsavedItems.Clear();


            foreach (var item in _changedUnsavedItems)
            {
                try { if (!await UpdateItem(item, tkn)) return false; }
                catch (Exception ex)
                { return LogError($"UpdateItem: [nid:{item?.nid}] «{item?.title}»", ex); }
            }
            _changedUnsavedItems.Clear();

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



        private void RaiseOneChangeCommitted()
            => OneChangeCommitted?.Invoke(this, EventArgs.Empty);


    }
}
