using System.Collections.Generic;
using ErrH.Tools.MvvmPattern;

namespace ErrH.WpfTools.CollectionShims
{
    public class GridVm<TMaster, TDetails> 
        : VmList<Selectable<MasterDetail<TMaster, TDetails>>>
    {


        public void Add(TMaster master, IEnumerable<TDetails> details)
        {
            base.Add(new Selectable<MasterDetail<TMaster, TDetails>>(new MasterDetail<TMaster, TDetails>(master, details)));
        }
    }
}
