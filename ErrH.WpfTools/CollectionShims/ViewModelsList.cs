using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using ErrH.WpfTools.ViewModels;

namespace ErrH.WpfTools.CollectionShims
{
    public class ViewModelsList<T> : Observables<T> where T : ViewModelBase
    {
        public SelectionMode SelectionMode { get; set; } = SelectionMode.Single;

        public ViewModelsList(List<T> list) : base(list ?? new List<T>())
        {
        }


        public int SelectedIndex {
            get { return SelectedItem?.ListIndex ?? -1; }
            set
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (i == value)
                        this[i].IsSelected = true;
                    else
                        this[i].IsSelected = false;
                }
            }
        }


        private void EnforceSelectionMode(T t)
        {
            //throw new NotImplementedException();
        }


        public List<T> SelectedItems => 
            this.Where(x => x.IsSelected == true).ToList();

        public T SelectedItem 
            => SelectedItems.FirstOrDefault();


    }
}
