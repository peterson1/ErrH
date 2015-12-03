using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using ErrH.Tools.Drupal7Models.Entities;
using ErrH.Tools.Loggers;

namespace ErrH.Tools.Drupal7Models
{
    public interface ID7Writer<T> : ILogSource 
        where T : ID7Node, new()
    {
        event EventHandler OneChangeCommitted;

        ID7Client              Client              { get; set; }
        ReadOnlyCollection<T>  NodesTracked        { get; }
        ReadOnlyCollection<T>  NewUnsavedItems     { get; }
        ReadOnlyCollection<T>  ChangedUnsavedItems { get; }
        T                      this [int nid]      { get; }
        Task<bool>             SaveChanges         (CancellationToken token = new CancellationToken());
        void                   AddLater            (T newNode);
        void                   TrackChanges        (IEnumerable<T> nodes);
    }
}
