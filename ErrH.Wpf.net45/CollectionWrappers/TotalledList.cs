using System.Collections.Specialized;
using System.Linq;
using ErrH.Tools.DomainHelpers;

namespace ErrH.Wpf.net45.CollectionWrappers
{
    public class TotalledList<T> : Selectables<T>
        where T : class, IWithTotal
    {

        public decimal  Total  { get; private set; }


        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);
            Total = this.Sum(x => x.Item?.Total ?? 0);
        }
    }
}
