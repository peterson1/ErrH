using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Data;
using ErrH.Tools.ErrorConstructors;

namespace ErrH.WpfTools.ViewModels
{
    public abstract class SlowListWvmBase<T> : WorkspaceViewModelBase where T : ViewModelBase
    {
        protected TaskCompletionSource<bool> _completion;


        public ObservableCollection<T> MainList { get; private set; }


        public SlowListWvmBase()
        {
            Refreshed += async (s, e) =>
            {
                IsBusy = true;
                _completion = new TaskCompletionSource<bool>();

                var list = await CreateVMsList();

                MainList = new ObservableCollection<T>(list);
                MainList.CollectionChanged += OnCollectionChanged;
                SortList();

                _completion = null;
                IsBusy = false;
            };
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
            FirePropertyChanged(e.PropertyName);
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
