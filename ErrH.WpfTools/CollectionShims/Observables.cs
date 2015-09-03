using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace ErrH.WpfTools.CollectionShims
{
    public class Observables<T> : ObservableCollection<T>
    {
        //public override event NotifyCollectionChangedEventHandler CollectionChanged;

        public Observables(List<T> list) : base(list) { }




        //public void Fire_CollectionChanged(object sender)
        //{
        //    CollectionChanged?.Invoke(sender, 
        //        new NotifyCollectionChangedEventArgs
        //            (NotifyCollectionChangedAction.Add));
        //}
    }
}



//todo: delete ErrH/ folder