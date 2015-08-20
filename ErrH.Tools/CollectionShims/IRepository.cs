using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ErrH.Tools.Loggers;
using ErrH.Tools.ScalarEventArgs;

namespace ErrH.Tools.CollectionShims
{
    public interface IRepository<T> : ILogSource
    {
        event EventHandler<EArg<T>>  Added;
        event EventHandler           Loaded;

        ReadOnlyCollection<T>  All  { get; }
                   
        bool  Add   (T itemToAdd);
        bool  Has   (T findThis);
        bool  Load  (string dataSourceUri);

        IEnumerable<T>  Any    (Func<T, bool> predicate);
        int             Count  (Func<T, bool> predicate);

    }
}
