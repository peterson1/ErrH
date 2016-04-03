using System.Collections.Generic;
using System.Linq;
using ErrH.Core.PCL45.Models;

namespace ErrH.Core.PCL45.Collections
{
    public class ObservaNum<T> : Observables<T>
        where T : IWithTotal
    {
        public ObservaNum()
        {
            SetChangeHandler();
        }


        public ObservaNum(IEnumerable<T> items) : base(items)
        {
            SetChangeHandler();
        }


        private void SetChangeHandler()
        {
            CollectionChanged += (s, e) => ComputeTotal();
            ComputeTotal();
        }

        private void ComputeTotal() =>
            Total = Count == 0 ? 0 : this.Sum(x => x.Total);

        public decimal Total { get; private set; }
    }
}
