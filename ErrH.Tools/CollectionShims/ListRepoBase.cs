using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ErrH.Tools.DataAttributes;
using ErrH.Tools.ErrorConstructors;
using ErrH.Tools.Extensions;
using ErrH.Tools.Loggers;
using ErrH.Tools.ScalarEventArgs;

namespace ErrH.Tools.CollectionShims
{
    public abstract class ListRepoBase<T> : LogSourceBase, IRepository<T>
    {
        public event EventHandler<EArg<T>> Added;
        public event EventHandler          Loaded;


        private List<T> _list = new List<T>();


        protected abstract Func<T, object> GetKey { get; }
        protected abstract bool LoadList(string dataSourceUri, ref List<T> list);


        public bool Load(string dataSourceUri)
        {
            try {
                if (!LoadList(dataSourceUri, 
                    ref _list)) return false;
            }
            catch (Exception ex)
            {
                return Error_n("Repo Load error.", 
                    ex.Details(false, false));
            }
            Loaded?.Invoke(this, EventArgs.Empty);
            return true;
        }



        public ReadOnlyCollection<T> All
            => new ReadOnlyCollection<T>(_list);


        public IEnumerable<T> Any(Func<T, bool> predicate)
            => _list.Where(predicate);


        public int Count(Func<T, bool> predicate)
            => _list.Count(predicate);




        public bool Has(T findThis)
        {
            if (findThis == null) return false;
            if (_list.Count == 0) return false;

            return Count(x => GetKey.Invoke(x).GetHashCode()
                           == GetKey.Invoke(findThis).GetHashCode()) != 0;
        }




        //protected void FireLoaded()
        //    => Loaded?.Invoke(this, EventArgs.Empty);




        public bool Add(T itemToAdd)
        {
            if (!DataError.IsBlank(itemToAdd))
                throw Error.BadAct("Attempted to Save() invalid Repo Item.");

            if (this.Has(itemToAdd)) return false;

            _list.Add(itemToAdd);
            Added?.Invoke(this, new EArg<T> { Value = itemToAdd });
            return true;
        }





    }
}
