﻿using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ErrH.WpfTools.CollectionShims
{
    public class Observables<T> : ObservableCollection<T>
    {
        //public override event NotifyCollectionChangedEventHandler CollectionChanged;


        public DataTableShim Sums { get; } = new DataTableShim();


        public Observables() : this(new List<T>()) { }


        public Observables(IEnumerable<T> list)
            : base(list ?? new List<T>())
        {
            CollectionChanged += (s, e) =>
                Sums.RaiseHostCollectionChanged(this);
        }


        public void Add(IEnumerable<T> items)
        {
            foreach (var item in items) Add(item);
        }


        /// <summary>
        /// Clears all existing items, then adds the new ones.
        /// </summary>
        /// <param name="items"></param>
        public void Swap(IEnumerable<T> items)
        {
            Clear();
            Add(items);
        }

        //public void Fire_CollectionChanged(object sender)
        //{
        //    CollectionChanged?.Invoke(sender, 
        //        new NotifyCollectionChangedEventArgs
        //            (NotifyCollectionChangedAction.Add));
        //}
    }
}


