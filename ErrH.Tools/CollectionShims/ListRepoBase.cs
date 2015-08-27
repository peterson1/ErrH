using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        private EventHandler            _cancelled;
        private EventHandler<EArg<int>> _retrying;

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
        public event EventHandler            Cancelled
        {
            add    { _cancelled -= value; _cancelled += value; }
            remove { _cancelled -= value; }
        }
        public event EventHandler<EArg<int>> Retrying
        {
            add    { _retrying -= value; _retrying += value; }
            remove { _retrying -= value; }
        }


        protected List<T> _list = new List<T>();
        //protected CancellationTokenSource _cancelSource = new CancellationTokenSource();

        protected abstract Func<T, object> GetKey { get; }
        protected abstract List<T> LoadList(object[] args);



        public virtual bool Load(params object[] args)
        {
            Fire_Loading();
            try {
                _list = LoadList(args);
            }
            catch (Exception ex)
            {
                return Error_n("Repo Load error.",
                    ex.Details(false, false));
            }
            if (_list == null) return false;
            Fire_Loaded();
            return true;
        }



        protected void Fire_Loading()
            => _loading?.Invoke(this, EventArgs.Empty);


        protected void Fire_Loaded()
            => _loaded?.Invoke(this, EventArgs.Empty);


        public ReadOnlyCollection<T> All
            => new ReadOnlyCollection<T>(_list);


        public T One(Func<T, bool> predicate)
            => _list.SingleOrDefault(predicate);


        public IEnumerable<T> Any(Func<T, bool> predicate)
            => _list.Where(predicate);


        public int Count(Func<T, bool> predicate)
            => _list.Count(predicate);


        public void Dispose() 
            => _list?.Clear();



        public bool Has(T findThis)
        {
            if (findThis == null) return false;
            if (_list.Count == 0) return false;

            return Count(x => GetKey.Invoke(x).GetHashCode()
                           == GetKey.Invoke(findThis).GetHashCode()) != 0;
        }




        public bool Add(T itemToAdd)
        {
            if (!DataError.IsBlank(itemToAdd))
                throw Error.BadAct("Attempted to Save() invalid Repo Item.");

            if (this.Has(itemToAdd)) return false;

            _list.Add(itemToAdd);
            _added?.Invoke(this, new EArg<T> { Value = itemToAdd });
            return true;
        }


        public void Cancel()
        {
            //_cancelSource.Cancel();
            _cancelled?.Invoke(this, EventArgs.Empty);
            Warn_n("User cancelled the operation.", "");
        }


        protected void FireRetrying(int seconds)
        {
            _retrying?.Invoke(this, NewArg(seconds));
        }
    }
}
