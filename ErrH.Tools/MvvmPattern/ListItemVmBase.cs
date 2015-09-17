namespace ErrH.Tools.MvvmPattern
{
    public abstract class ListItemVmBase : ViewModelBase
    {

        private bool _isSelected;
        public  bool  IsSelected
        {
            get { return _isSelected; }
            set { SetField(ref _isSelected, value, nameof(IsSelected)); }
        }


        //private int _listIndex;
        //public int ListIndex
        //{
        //    get { return _listIndex; }
        //    set { SetField(ref _listIndex, value, nameof(ListIndex)); }
        //}

    }
}
