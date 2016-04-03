using System.Linq;
using ErrH.Core.PCL45.Models;
using PropertyChanged;

namespace ErrH.Wpf.net45.CollectionWrappers
{
    [ImplementPropertyChanged]
    public class TotalledList<T> : Selectables<T>
        where T : class, IWithTotal
    {
        public TotalledList()
        {
            CollectionChanged += (s, e) =>
            {
                if (this.Count == 0)
                    Total = 0;
                else
                    Total = this.Sum(x => x.Item?.Total ?? 0);
            };
        }


        public decimal  Total  { get; private set; }


        //protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        //{
        //    base.OnCollectionChanged(e);

        //    if (this.Count == 0)
        //        Total = 0;
        //    else
        //        Total = this.Sum(x => x.Item?.Total ?? 0);
        //}
    }
}
