using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using ErrH.Tools.ErrorConstructors;


namespace ErrH.WpfTools.ViewModels
{
    public abstract class ListWorkspaceVMBase<T> : WorkspaceViewModelBase where T : ViewModelBase
    {
        private ObservableCollection<T> _mainList;
        public  ObservableCollection<T>  MainList
        {
            get
            {
                if (_mainList != null) return _mainList;
                var list = DefineListItems();
                _mainList = new ObservableCollection<T>(list);
                _mainList.CollectionChanged += OnCollectionChanged;
                return _mainList;
            }
        }


        protected abstract List<T> DefineListItems();


        protected void RefreshVMList()
        {
            _mainList = null;
            OnPropertyChanged(nameof(MainList));
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
            var vm = sender as T;
            Throw.IfNull(vm, $"Expected sender to be ‹{typeof(T).Name}›.");

            OnPropertyChanged(e.PropertyName);
        }


        protected override void OnDispose()
        {
            foreach (var vm in MainList)
                vm.Dispose();

            MainList.Clear();
            MainList.CollectionChanged -= OnCollectionChanged;
        }
    }
}
