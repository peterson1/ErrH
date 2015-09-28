using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ErrH.Tools.Authentication;
using ErrH.Tools.Loggers;
using ErrH.Tools.ScalarEventArgs;

namespace ErrH.Tools.CollectionShims
{
    public interface IRepository<T> : ILogSource, IDisposable
    {
        event EventHandler<EArg<T>>   Added;
        event EventHandler            Loading;
        event EventHandler            Loaded;
        event EventHandler            SavingChanges;
        event EventHandler            ChangesSaved;
        event EventHandler            Cancelled;
        event EventHandler<EArg<int>> DelayingRetry;

        ReadOnlyCollection<T>  All  { get; }


        /// <summary>
        /// Wrapper for Select().ToList()
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="selector"></param>
        /// <returns></returns>
        List<TResult>   Select<TResult>(Func<T, TResult> selector);

        List<TResult>   Select<TResult>(Func<T, int, TResult> selector);


        bool  Add         (T itemToAdd);
        bool  Has         (T findThis);
        bool  Load        (params object[] args);
        bool  SaveChanges ();

        int Length { get; }

        Task<bool>  LoadAsync (params object[] args);


        /// <summary>
        /// Wrapper for SingleOrDefault().
        /// Returns the only element of a sequence, or NULL if the sequence is empty.
        /// This method throws an exception if there is more than one element in the sequence.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        T One(Func<T, bool> predicate);

        IEnumerable<T>  Any    (Func<T, bool> predicate);
        int             Count  (Func<T, bool> predicate);



        T this[int index] { get; }


        /// <summary>
        /// Raises the Cancelled event.
        /// </summary>
        void RaiseCancelled();


        void SetClient(ISessionClient sessionClient, LoginCredentials credentials);
    }
}
