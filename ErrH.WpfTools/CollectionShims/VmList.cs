using System.Collections.Generic;
using System.Linq;
using ErrH.Tools.MvvmPattern;

namespace ErrH.WpfTools.CollectionShims
{
    public class VmList<T> : Observables<T> where T : ListItemVmBase
    {
        //public SelectionMode SelectionMode { get; set; } = SelectionMode.Single;

        public VmList(List<T> list) : base(list ?? new List<T>())
        {
        }


        //public int SelectedIndex {
        //    get { return SelectedItem?.ListIndex ?? -1; }
        //    set
        //    {
        //        for (int i = 0; i < this.Count; i++)
        //        {
        //            if (i == value)
        //                this[i].IsSelected = true;
        //            else
        //                this[i].IsSelected = false;
        //        }
        //    }
        //}



        public List<T> SelectedItems => 
            this.Where(x => x.IsSelected == true).ToList();

        public T SelectedItem 
            => SelectedItems.FirstOrDefault();

        public bool HasSelection => SelectedItem != null;
    }
}
