using System.Collections.ObjectModel;
using System.Windows.Data;

namespace ErrH.Wpf.net45.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static void SetCurrent<T>(this ObservableCollection<T> colxn, T item)
        {
            var colxnView = CollectionViewSource.GetDefaultView(colxn);
            colxnView?.MoveCurrentTo(item);
        }

        public static void SetCurrent<T>(this ObservableCollection<T> colxn, int itemIndex)
            => colxn.SetCurrent(colxn[itemIndex]);
    }
}
