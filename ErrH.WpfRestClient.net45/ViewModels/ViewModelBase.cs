using System.Threading;
using PropertyChanged;

namespace ErrH.WpfRestClient.net45.ViewModels
{
    [ImplementPropertyChanged]
    public class ViewModelBase
    {
        protected SynchronizationContext _ui;


        public ViewModelBase()
        {
            _ui = SynchronizationContext.Current;
        }

        protected void AsUI (SendOrPostCallback action)
            => _ui.Send(action, null);

    }
}
