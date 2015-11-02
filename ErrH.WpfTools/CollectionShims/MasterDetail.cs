using System.Collections.ObjectModel;

namespace ErrH.WpfTools.CollectionShims
{
    public class MasterDetail<TMaster, TDetail> : ObservableCollection<TMaster>
    {
        public TMaster Master { get; set; }
        public ObservableCollection<TDetail> Details { get; set; }
    }
}
