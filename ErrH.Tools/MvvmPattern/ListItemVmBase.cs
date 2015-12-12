using System;
using ErrH.Tools.ScalarEventArgs;

namespace ErrH.Tools.MvvmPattern
{
    public abstract class ListItemVmBase : ViewModelBase
    {
        private      EventHandler<EArg<bool>> _isSelectedChanged;
        public event EventHandler<EArg<bool>>  IsSelectedChanged
        {
            add    { _isSelectedChanged -= value; _isSelectedChanged += value; }
            remove { _isSelectedChanged -= value; }
        }


        private bool _isSelected;
        public  bool  IsSelected
        {
            get { return _isSelected; }
            set { SetField(ref _isSelected, value, nameof(IsSelected)); }
        }

        private bool _isChecked;
        public  bool  IsChecked
        {
            get { return _isChecked; }
            set { SetField(ref _isChecked, value, nameof(IsChecked)); }
        }



        public ListItemVmBase()
        {
            this.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(IsSelected))
                {
                    _isSelectedChanged?.Invoke(s, 
                        new EArg<bool> { Value = IsSelected });
                }
            };
        }


        //private int _listIndex;
        //public int ListIndex
        //{
        //    get { return _listIndex; }
        //    set { SetField(ref _listIndex, value, nameof(ListIndex)); }
        //}

    }
}
