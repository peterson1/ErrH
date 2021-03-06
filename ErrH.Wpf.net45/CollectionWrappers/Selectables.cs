﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using ErrH.Core.PCL45.Collections;
using ErrH.Tools.ScalarEventArgs;
using PropertyChanged;

namespace ErrH.Wpf.net45.CollectionWrappers
{
    [ImplementPropertyChanged]
    public class Selectables<T> : Observables<Selectable<T>>
        where T : class
    {
        private      EventHandler<EArg<T>> _selectionChanged;
        public event EventHandler<EArg<T>>  SelectionChanged
        {
            add    { _selectionChanged -= value; _selectionChanged += value; }
            remove { _selectionChanged -= value; }
        }

        public T SelectedItem { get; private set; }

        public bool IsBusy => this.Any(x => x.IsBusy);

        public void RefreshView() =>
            CollectionViewSource.GetDefaultView(this)?.Refresh();


        public IEnumerable<T> GetItems()
        {
            var list = new List<T>();
            foreach (var item in this)
            {
                if (item.Item != null)
                    list.Add(item.Item);
            }
            return list;
        }


        public bool MakeCurrent(Selectable<T> selectable)
        {
            var clxnVw = CollectionViewSource.GetDefaultView(this);
            if (clxnVw == null) return false;
            return clxnVw.MoveCurrentTo(selectable);
        }


        public bool MakeCurrent(T item)
        {
            var slctbl = this.SingleOrDefault(_ => _.Item.Equals(item));
            if (slctbl == null) return false;
            slctbl.IsSelected = true;
            SelectedItem = slctbl.Item;
            return MakeCurrent(slctbl);
        }


        public void Add(IEnumerable<T> items)
        {
            if (items == null) return;
            foreach (var item in items)
            {
                var selctbl = new Selectable<T>(item);

                selctbl.PropertyChanged += Selctbl_PropertyChanged;

                Add(selctbl);
            }
        }

        public void Add(T item) => Add(new Selectable<T>(item));



        private void Selctbl_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(Selectable<T>.IsSelected)) return;
            var item = sender as Selectable<T>;
            if (item == null) return;
            if (item.IsSelected)
                _selectionChanged?.Invoke(this, 
                    new EArg<T> { Value = SelectedItem = item.Item });
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
    }
}
