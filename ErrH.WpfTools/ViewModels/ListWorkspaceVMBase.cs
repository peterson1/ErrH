using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Data;
using ErrH.Tools.ErrorConstructors;


namespace ErrH.WpfTools.ViewModels
{
    public abstract class ListWorkspaceVMBase<T> : WorkspaceViewModelBase where T : ViewModelBase
    {

        public ObservableCollection<T> MainList { get; private set; }

        protected void RefreshVMList()
        {
            MainList = new ObservableCollection<T>(DefineListItems());
            MainList.CollectionChanged += OnCollectionChanged;
        }


        protected abstract List<T> DefineListItems();

        public void SortBy(string colName, 
            ListSortDirection order = ListSortDirection.Ascending)
        {
            var vs = CollectionViewSource.GetDefaultView(MainList);
            vs.SortDescriptions.Add(new SortDescription(colName, order));
            vs.Refresh();
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

            FirePropertyChanged(e.PropertyName);
        }


        protected override void OnDispose()
        {
            if (MainList == null) return;
            foreach (var vm in MainList)
                vm.Dispose();

            MainList.Clear();
            MainList.CollectionChanged -= OnCollectionChanged;
        }
    }
}
