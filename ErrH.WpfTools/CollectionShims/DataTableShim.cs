using System;
using System.Data;

namespace ErrH.WpfTools.CollectionShims
{
    public class DataTableShim : DataTable
    {
        public event EventHandler HostCollectionChanged;


        public void RaiseHostCollectionChanged(object sender) 
            => HostCollectionChanged?.Invoke(sender, EventArgs.Empty);
    }
}
