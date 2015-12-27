using System;
using System.ComponentModel;
using System.Windows.Controls;
using ErrH.Tools.ErrorConstructors;
using ErrH.Tools.Loggers;
using ErrH.Tools.MvvmPattern;
using ErrH.WpfTools.Commands;
using ErrH.WpfTools.Extensions;
using ErrH.WpfTools.PrintHelpers;
using PropertyChanged;

namespace ErrH.WpfTools.ViewModels
{
    [ImplementPropertyChanged]
    public abstract class WorkspaceVmBase : ListItemVmBase, INotifyPropertyChanged
    {
        private      EventHandler _closed;
        public event EventHandler  Closed
        {
            add    { _closed -= value; _closed += value; }
            remove { _closed -= value; }
        }

        private      EventHandler _refreshed;
        public event EventHandler  Refreshed
        {
            add    { _refreshed -= value; _refreshed += value; }
            remove { _refreshed -= value; }
        }

        private      EventHandler _cancelled;
        public event EventHandler  Cancelled
        {
            add    { _cancelled -= value; _cancelled += value; }
            remove { _cancelled -= value; }
        }


        private int? _hashCode;
        //private volatile bool _currentlyRefreshing;
        private long _lastRefresh;


        //public bool    IsBusy          { get; protected set; }
        public bool    IsDelayingRetry { get; protected set; }
        public string  BusyText        { get; protected set; } = "Please wait ...";
        public string  RetryingText    { get; protected set; }
        public L4j     MessageTone     { get; protected set; } = L4j.Info;
        public RelayCommand PrintCommand => CreatePrintCommand();


        //public MainWindowVMBase  ParentWindow  { get; set; }

        public void Close   () => CloseCommand  .ExecuteIfItCan();
        public void Refresh ()
        {
            if (RefreshedTooSoon()) return;

            try {  RefreshCommand.ExecuteIfItCan(TriggeredBy.Code);  }
            catch (Exception ex) {  LogError("RefreshCommand", ex);  }
        }


        private bool RefreshedTooSoon(int seconds = 1)
        {
            var tixNow = DateTime.Now.Ticks;
            var elapsd = (tixNow - _lastRefresh);
            _lastRefresh = tixNow;
            return elapsd < (1000000 * 10 * seconds);
        }






        private RelayCommand _closeCmd;
        public  RelayCommand  CloseCommand
        {
            get
            {
                if (_closeCmd != null) return _closeCmd;
                _closeCmd = new RelayCommand(
                    x => _closed?.Invoke(this, EventArgs.Empty), 
                    x => !IsBusy);
                return _closeCmd;
            }
        }

        private RelayCommand _refreshCmd;
        public  RelayCommand  RefreshCommand
        {
            get
            {
                if (_refreshCmd != null) return _refreshCmd;
                _refreshCmd = new RelayCommand(x => 
                {
                    var trigrdBy = x == null ? TriggeredBy.User
                        : (TriggeredBy)Enum.Parse(typeof(TriggeredBy), x.ToString());

                    try {  OnRefresh(trigrdBy);  }
                    catch (Exception ex) { LogError("OnRefresh(trigrdBy)", ex); }
                    
                    try {  _refreshed?.Invoke(this, EventArgs.Empty);  }
                    catch (Exception ex) { LogError("_refreshed.Invoke", ex); }
                }, 
                x => !IsBusy);
                return _refreshCmd;
            }
        }

        private RelayCommand _cancelCmd;
        public  RelayCommand  CancelCommand
        {
            get
            {
                if (_cancelCmd != null) return _cancelCmd;
                _cancelCmd = new RelayCommand(
                    x => {
                        _cancelled?.Invoke(this, EventArgs.Empty);
                        IsBusy = false;
                    },
                    x => IsDelayingRetry);
                return _cancelCmd;
            }
        }


        protected virtual void OnRefresh(TriggeredBy triggeredBy) { }


        public virtual void SetIdentifier(object identifier)
        {
            if (_hashCode.HasValue) Throw.BadAct("Identifier can only be set once.");
            _hashCode = HashCodeFor(identifier);
        }


        public virtual int HashCodeFor(object identifier) =>
            (GetType().Name + identifier.GetHashCode()).GetHashCode();


        public override int GetHashCode() 
            => _hashCode.GetValueOrDefault(0);



        private RelayCommand CreatePrintCommand()
            => new RelayCommand(x => OnPrint(AsTabContent(x)), 
                                x => AsTabContent(x) != null);


        protected virtual void OnPrint(ContentPresenter contentPresenter)
        {
            //BenWalkerPrinter.AskToPrint(contentPresenter, this);
            //DirectVisualPrinter.AskToPrint(contentPresenter);
            ScaledVisualPrinter.AskToPrint(contentPresenter, this);
        }


        private ContentPresenter AsTabContent(object x)
        {
            var tabC = x as TabControl;
            var cont = tabC?.Find<ContentPresenter>("PART_SelectedContentHost");
            return cont;
        }




    }

}
