using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using ErrH.Tools.Authentication;
using ErrH.Tools.Loggers;
using ErrH.Tools.ScalarEventArgs;

namespace ErrH.Tools.CollectionShims
{
    public interface IRepository<T> : ILogSource, IClientSource, IDisposable
    {
        event EventHandler<EArg<T>>   Added;
        event EventHandler            Loading;
        event EventHandler            Loaded;
        event EventHandler            SavingChanges;
        event EventHandler            ChangesSaved;
        event EventHandler            Cancelled;
        event EventHandler<EArg<int>> DelayingRetry;

        event EventHandler<EArg<ReadOnlyCollection<T>>> DataChanged;

        List<T> NewUnsavedItems     { get; }
        List<T> ChangedUnsavedItems { get; }


        ReadOnlyCollection<T>  All  { get; }


        /// <summary>
        /// Wrapper for Select().ToList()
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="selector"></param>
        /// <returns></returns>
        List<TResult>   Select<TResult>(Func<T, TResult> selector);

        List<TResult>   Select<TResult>(Func<T, int, TResult> selector);


        bool  Has         (T findThis);
        bool  Load        (params object[] args);


        bool       Add              (T itemToAdd);
        bool       SaveChanges      ();
        Task<bool> SaveChangesAsync (CancellationToken tkn = new CancellationToken());


        int Length { get; }

        Task<bool> LoadAsync(CancellationToken tkn, params object[] args);


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


        void SetClient(ISessionClient sessionClient, IBasicAuthenticationKey credentials);
        void ShareClientWith<TAny>(IRepository<TAny> anotherRepo);
    }
}
