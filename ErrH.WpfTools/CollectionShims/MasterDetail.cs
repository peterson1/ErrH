using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ErrH.WpfTools.CollectionShims
{
    public class MasterDetail<TMaster, TDetail> : ObservableCollection<TMaster>
    {
        public TMaster              Master  { get; set; }
        public Observables<TDetail> Details { get; set; }


        public MasterDetail()
        {
        }

        public MasterDetail(TMaster master, IEnumerable<TDetail> details)
        {
            Master = master;
            Details = new Observables<TDetail>(details);
        }
    }
}
