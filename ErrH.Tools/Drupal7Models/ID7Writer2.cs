using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ErrH.Tools.Drupal7Models.Entities;
using ErrH.Tools.Loggers;

namespace ErrH.Tools.Drupal7Models
{
    public interface ID7Writer2<T> : ILogSource, IDisposable
        where T : ID7Node, new()
    {
        ID7Client       Client        { get; set; }
        string          TaskTitle     { get; set; }
        IEnumerable<T>  TrackChanges  (IEnumerable<T> nodes);
        T               Tracked       (int nid);
        IEnumerable<T>  Tracked       (Func<T, bool> filter);
        bool            HasChanges    { get; }
        Task<bool>      SaveChanges   (CancellationToken token = new CancellationToken());
        void            AddLater      (T newNode);
        void            DeleteLater   (T unwantedNode);
        bool            WillAdd       (Func<T, bool> filter);
        bool            WillEdit      (Func<T, bool> filter);
        bool            WillDelete    (Func<T, bool> filter);
    }
}
