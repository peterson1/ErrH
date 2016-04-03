using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ErrH.Core.PCL45.Collections
{
    public class Observables<T> : ObservableCollection<T>
    {
        public Observables() { }
        public Observables(IEnumerable<T> list) : base(list) { }


        public Observables<T> SummaryRow { get; set; }


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
