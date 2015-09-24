using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using ErrH.Tools.Extensions;
using ErrH.Tools.MvvmPattern;
using ErrH.Tools.ScalarEventArgs;

namespace ErrH.WpfTools.CollectionShims
{
    public class VmList<T> : Observables<T>, IDisposable
        where T : ListItemVmBase
    {
        private      EventHandler<EArg<T>> _itemPicked;
        public event EventHandler<EArg<T>>  ItemPicked
        {
            add    { _itemPicked -= value; _itemPicked += value; }
            remove { _itemPicked -= value; }
        }



        public VmList(List<T> list = null) : base(list)
        {
            if (list == null) list = new List<T>();

            foreach (var item in list)
                item.PropertyChanged += OnItemPropertyChanged;

            CollectionChanged += OnCollectionChanged;
        }


        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (T vm in e.NewItems)
                    vm.PropertyChanged += OnItemPropertyChanged; ;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (T vm in e.OldItems)
                    vm.PropertyChanged -= OnItemPropertyChanged;
        }


        private void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var vm = sender.As<T>();
            if (e.PropertyName == nameof(vm.IsSelected) && vm.IsSelected)
                _itemPicked?.Invoke(sender, EArg<T>.NewArg(vm));
        }



        public List<T> CheckedItems
            => this.Where(x => x.IsChecked == true).ToList();



        public List<T> SelectedItems 
            => this.Where(x => x.IsSelected == true).ToList();


        public T SelectedItem 
            => SelectedItems.FirstOrDefault();


        public bool HasSelection 
            => SelectedItem != null;


        public void SelectOne(int index)
        {
            if (Count == 0) return;
            this.ForEach(x => x.IsSelected = false);
            if (index == -1) return;

            if (index > -1 && index < Count)
                this[index].IsSelected = true;
        }


        public virtual void Dispose()
        {
            CollectionChanged -= OnCollectionChanged;
            this.ForEach(x => x.Dispose());
            this.Clear();
        }

    }
}
