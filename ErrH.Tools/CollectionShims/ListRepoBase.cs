using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ErrH.Tools.Authentication;
using ErrH.Tools.DataAttributes;
using ErrH.Tools.ErrorConstructors;
using ErrH.Tools.Extensions;
using ErrH.Tools.Loggers;
using ErrH.Tools.ScalarEventArgs;
using static ErrH.Tools.ScalarEventArgs.EArg<int>;

namespace ErrH.Tools.CollectionShims
{
    public abstract class ListRepoBase<T> : LogSourceBase, IRepository<T>
    {
        private EventHandler<EArg<T>>   _added;
        private EventHandler            _loading;
        private EventHandler            _loaded;
        private EventHandler            _savingChanges;
        private EventHandler            _changesSaved;
        private EventHandler            _cancelled;
        private EventHandler<EArg<int>> _delayingRetry;

        public event EventHandler<EArg<T>>   Added
        {
            add    { _added -= value; _added += value; }
            remove { _added -= value; }
        }
        public event EventHandler            Loading
        {
            add    { _loading -= value; _loading += value; }
            remove { _loading -= value; }
        }
        public event EventHandler            Loaded
        {
            add    { _loaded -= value; _loaded += value; }
            remove { _loaded -= value; }
        }
        public event EventHandler SavingChanges
        {
            add    { _savingChanges -= value; _savingChanges += value; }
            remove { _savingChanges -= value; }
        }
        public event EventHandler ChangesSaved
        {
            add    { _changesSaved -= value; _changesSaved += value; }
            remove { _changesSaved -= value; }
        }
        public event EventHandler            Cancelled
        {
            add    { _cancelled -= value; _cancelled += value; }
            remove { _cancelled -= value; }
        }
        public event EventHandler<EArg<int>> DelayingRetry
        {
            add    { _delayingRetry -= value; _delayingRetry += value; }
            remove { _delayingRetry -= value; }
        }



        /// <summary>
        /// The internal storage.
        /// </summary>
        protected List<T> _list = new List<T>();
        //later: make this private



        public ListRepoBase() { }

        public ListRepoBase(List<T> initialItems)
        {
            _list = initialItems;
        }


        protected abstract Func<T, object> GetKey { get; }
        protected abstract List<T> LoadList(object[] args);



        public virtual void SetClient(ISessionClient sessionClient, 
                                      LoginCredentials credentials)
        {
        }


        public bool Load(params object[] args)
        {
            RaiseLoading();
            try {
                _list = LoadList(args);
            }
            catch (Exception ex)
            {
                return Error_n("Repo Load error.",
                    ex.Details(false, false));
            }
            if (_list == null) return false;
            RaiseLoaded();
            return true;
        }


        public virtual Task<bool> LoadAsync(params object[] args)
        {
            throw Error.Undone("LoadAsync", $"Implementation for ‹{GetType().Name}›.");
        }





        protected void RaiseSavingChanges()
            => _savingChanges?.Invoke(this, EventArgs.Empty);


        protected void RaiseChangesSaved()
            => _changesSaved?.Invoke(this, EventArgs.Empty);


        protected void RaiseLoading()
            => _loading?.Invoke(this, EventArgs.Empty);


        protected void RaiseLoaded()
            => _loaded?.Invoke(this, EventArgs.Empty);


        protected void RaiseDelayingRetry(int seconds)
            => _delayingRetry?.Invoke(this, NewArg(seconds));


        protected void RaiseAdded(T newItem)
            => _added?.Invoke(this, new EArg<T> { Value = newItem });


        public void RaiseCancelled()
        {
            _cancelled?.Invoke(this, EventArgs.Empty);
            Warn_n("User cancelled the operation.", "");
        }






        public ReadOnlyCollection<T> All
            => new ReadOnlyCollection<T>(_list);


        public int Length => _list?.Count ?? -1;


        public T this[int index] => _list[index];


        public T One(Func<T, bool> predicate)
        {
            if (_list == null) return default(T);
            return _list.SingleOrDefault(predicate);
        }
            //=> _list.SingleOrDefault(predicate);


        public IEnumerable<T> Any(Func<T, bool> predicate)
            => _list.Where(predicate);


        public int Count(Func<T, bool> predicate)
            => _list.Count(predicate);


        public List<TResult> Select<TResult>(Func<T, TResult> selector)
            => _list.Select(selector).ToList();


        public List<TResult> Select<TResult>(Func<T, int, TResult> selector)
            => _all.Select(selector).ToList();

        public void Dispose() 
            => _list?.Clear();



        public bool Has(T findThis)
        {
            if (findThis == null) return false;
            if (_list.Count == 0) return false;

            return Count(x => GetKey.Invoke(x).GetHashCode()
                           == GetKey.Invoke(findThis).GetHashCode()) != 0;
        }




        public virtual bool Add(T itemToAdd)
        {
            if (!DataError.IsBlank(itemToAdd))
                throw Error.BadAct("Attempted to Save() invalid Repo Item.");

            if (this.Has(itemToAdd)) return false;

            _list.Add(itemToAdd);
            RaiseAdded(itemToAdd);
            return true;
        }





        public virtual bool SaveChanges()
        {
            return true;
        }


        private List<T> _all
        {
            get
            {
                if (_list == null) throw Error.NullRef(
                    $"Internal _list of ListRepoBase‹{typeof(T).Name}›");

                return _list;
            }
        }
    }
}
