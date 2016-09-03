using System.ComponentModel;
using PropertyChanged;

namespace ErrH.Core.PCL45.Collections
{
    [ImplementPropertyChanged]
    public class Selectable<T> : INotifyPropertyChanged
    {
        public Selectable(T item)
        {
            Item = item;
        }

        private PropertyChangedEventHandler     _propertyChanged;
        public event PropertyChangedEventHandler PropertyChanged
        {
            add    { _propertyChanged -= value; _propertyChanged += value; }
            remove { _propertyChanged -= value; }
        }


        public T       Item        { get; set; }
        public bool    IsSelected  { get; set; }
        public bool    IsBusy      { get; set; }
        public string  Status      { get; set; }


        public override string ToString() => Item?.ToString();
    }
}
