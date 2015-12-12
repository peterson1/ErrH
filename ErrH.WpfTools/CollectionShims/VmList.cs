using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Data;
using ErrH.Tools.Extensions;
using ErrH.Tools.MvvmPattern;
using ErrH.Tools.ScalarEventArgs;
using ErrH.WpfTools.ViewModels;

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


        public new T Add(T itemToAdd)
        {
            base.Add(itemToAdd);

            //if (SelectedItem == null) SelectOne(0);

            return itemToAdd;
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


        public T SelectOne(int index)
        {
            if (Count == 0) return default(T);
            SelectNone();
            if (index == -1) return default(T);

            if (index > -1 && index < Count)
                this[index].IsSelected = true;

            return this[index];
        }


        public bool MakeCurrent(ListItemVmBase vm)
        {
            if (vm.IsSelected) return true;
            Debug.Assert(this.Contains(vm));

            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this);
            if (collectionView == null) return false;

            return collectionView.MoveCurrentTo(vm);
            //if (!found)
            //    Warn_n($"{GetType().Name} : MakeCurrent({vmList})",
            //            $"Workspace not found: “{vm}”");
        }



        public void SelectNone()
        {
            if (Count == 0) return;
            this.ForEach(x => x.IsSelected = false);
        }



        public virtual void Dispose()
        {
            CollectionChanged -= OnCollectionChanged;
            this.ForEach(x => x.Dispose());
            this.Clear();
        }

    }
}
