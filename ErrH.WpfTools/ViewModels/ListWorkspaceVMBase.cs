using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Data;
using ErrH.Tools.ErrorConstructors;
using ErrH.Tools.Extensions;
using ErrH.Tools.MvvmPattern;
using ErrH.Tools.ScalarEventArgs;
using ErrH.WpfTools.CollectionShims;

namespace ErrH.WpfTools.ViewModels
{
    public abstract class ListWorkspaceVMBase<T> : WorkspaceViewModelBase where T : ListItemVmBase
    {
        private      EventHandler<EArg<T>> _itemPicked;
        public event EventHandler<EArg<T>>  ItemPicked
        {
            add    { _itemPicked -= value; _itemPicked += value; }
            remove { _itemPicked -= value; }
        }



        public ViewModelsList<T> MainList { get; private set; }


        public ListWorkspaceVMBase()
        {
            Refreshed += OnRefreshed;
        }

        private async void OnRefreshed(object sender, EventArgs e)
        {
            IsBusy = true;

            try
            {
                MainList = await GetViewModelsList();
            }
            catch (Exception ex)
            {
                Error_n($"Error ‹{typeof(T).Name}› OnRefreshed()", ex.Details());
                return;
            }
            if (MainList == null) return;

            MainList.CollectionChanged += OnCollectionChanged;

            foreach (var vm in MainList)
                vm.PropertyChanged += OnItemPropertyChanged;

            SortList();

            if (MainList?.Count != 0 && MainList.SelectedIndex == -1)
                MainList.SelectedIndex = 0;

            IsBusy = false;
        }



        private async Task<ViewModelsList<T>> GetViewModelsList()
        {
            List<T> list = null;
            var ret      = new ViewModelsList<T>(new List<T>());
            var method   = $"{GetType().Name}.CreateVMsList()";
            var retTyp   = $"List‹{typeof(T).Name}›";
            var errMsg   = $"Failed to get {retTyp} from {method}.";

            try {
                list = await CreateVMsList();
            }
            catch (Exception ex) {
                return Error_(ret, errMsg, ex.Details()); }

            if (list == null)
                return Error_(ret, errMsg, "Method returned NULL.");

            return new ViewModelsList<T>(list);
        }



        protected abstract Task<List<T>> CreateVMsList();

        protected virtual void SortList() { }



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
            RaisePropertyChanged(e.PropertyName);

            if (e.PropertyName == nameof(vm.IsSelected) && vm.IsSelected)
                _itemPicked?.Invoke(sender, EArg<T>.NewArg(vm));
        }


        protected void SortBy(string colName, ListSortDirection order = ListSortDirection.Ascending)
        {
            var vs = CollectionViewSource.GetDefaultView(MainList);
            vs.SortDescriptions.Add(new SortDescription(colName, order));
            vs.Refresh();
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
