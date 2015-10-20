using PropertyChanged;

namespace ErrH.Tools.MvvmPattern
{
    public class Selectable<T> : ListItemVmBase
    {
        public T Item { get; }



        public Selectable(T item)
        {
            Item = item;
        }

        public override string DisplayName => Item.ToString();
    }
}
