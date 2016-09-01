using System.Threading;
using PropertyChanged;

namespace ErrH.WpfRestClient.net45.ViewModels
{
    [ImplementPropertyChanged]
    public class ViewModelBase
    {
        protected SynchronizationContext _ui;


        public virtual string  Title      { get; set; }
        public virtual bool    IsBusy     { get; set; }
        public virtual string  BusyText   { get; set; }
        public virtual bool    IsVisible  { get; set; } = true;

        public ViewModelBase()
        {
            _ui = SynchronizationContext.Current;
        }

        protected void AsUI (SendOrPostCallback action)
            => _ui.Send(action, null);

    }
}
