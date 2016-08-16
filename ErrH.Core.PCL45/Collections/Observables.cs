using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ErrH.Core.PCL45.Extensions;

namespace ErrH.Core.PCL45.Collections
{
    public class Observables<T> : ObservableCollection<T>
    {
        public Observables() { }
        public Observables(IEnumerable<T> list) : base(list) { }


        private      EventHandler _rowsSummarized;
        public event EventHandler  RowsSummarized
        {
            add    { _rowsSummarized -= value; _rowsSummarized += value; }
            remove { _rowsSummarized -= value; }
        }


        private Observables<T> _summaryRow;

        public Observables<T> SummaryRow
        {
            get { return _summaryRow;  }
            set { _summaryRow = value; _rowsSummarized?.Raise(); }
        }


        public void Add(IEnumerable<T> items)
        {
            if (items == null) return;
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
    }
}
