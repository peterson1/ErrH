using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ErrH.Tools.Loggers;
using ErrH.Tools.ScalarEventArgs;

namespace ErrH.Tools.CollectionShims
{
    public interface IRepository<T> : ILogSource, IDisposable
    {
        event EventHandler<EArg<T>>  Added;
        event EventHandler           Loaded;

        ReadOnlyCollection<T>  All  { get; }
                   
        bool  Add  (T itemToAdd);
        bool  Has  (T findThis);
        bool  Load (params object[] args);

        IEnumerable<T>  Any    (Func<T, bool> predicate);
        int             Count  (Func<T, bool> predicate);

    }
}
