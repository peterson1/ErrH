using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
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
        protected SynchronizationContext _ui = SynchronizationContext.Current;


        //public bool    IsBusy          { get; protected set; }
        public bool    IsPrintable     { get; protected set; }
        public bool    IsRefreshable   { get; protected set; }
        public bool    IsClosable      { get; protected set; }
        public bool    IsDelayingRetry { get; protected set; }
        public string  BusyText        { get; protected set; } = "Please wait ...";
        public string  RetryingText    { get; protected set; }
        public L4j     MessageTone     { get; protected set; } = L4j.Info;
        public RelayCommand PrintCommand => CreatePrintCommand();

        public double PrintScaleOffset { get; protected set; } = 0;


        //public MainWindowVMBase  ParentWindow  { get; set; }

        public void Close   () => CloseCommand  .ExecuteIfItCan();
        public void Refresh ()
        {
            //if (RefreshedTooSoon()) return;

            try {  RefreshCommand.ExecuteIfItCan(TriggeredBy.Code);  }
            catch (Exception ex) {  LogError("RefreshCommand", ex);  }
        }


        protected bool RefreshedTooSoon(int milliseconds = 100)
        {
            var tixNow = DateTime.Now.Ticks;
            var elapsd = (tixNow - _lastRefresh);
            _lastRefresh = tixNow;
            return elapsd < (10000 * milliseconds);
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
            ScaledVisualPrinter.AskToPrint(contentPresenter, this, PrintScaleOffset);
        }


        private ContentPresenter AsTabContent(object obj)
        {
            if (obj == null) return null;

            if (obj is ContextMenu) return FromContextMenu(obj);

            var tabC = obj as FrameworkElement;
            if (tabC == null) return null;

            ContentPresenter cont = null;
            var found = false;
            try {
                found = tabC.TryFindChild<ContentPresenter>(x
                    => x.Name == "PART_SelectedContentHost", out cont);
            }
            catch (Exception ex){ LogError("tabC.TryFindChild<ContentPresenter>(x=> x.Name == 'PART_SelectedContentHost'", ex); }

            if (!found)
                Warn_n("Missing ContentPresenter “PART_SelectedContentHost”", $"“{tabC.Name}” ‹{tabC.GetType().Name}›");

            return cont;
        }


        private ContentPresenter FromContextMenu(object obj)
        {
            var cm = obj as ContextMenu;
            if (cm == null) return null;

            var targ = cm.PlacementTarget as FrameworkElement;
            if (targ == null) return null;

            var parent = (targ.Parent as FrameworkElement).Parent;
            if (parent == null) return null;

            ContentPresenter cont = null;
            var found = false;
            try {
                found = parent.TryFindChild<ContentPresenter>(out cont);
            }
            catch (Exception ex){ LogError("parent.TryFindChild<ContentPresenter>", ex); }

            if (!found)
                Warn_n("ContentPresenter missing from cm.PlacementTarget.Parent",
                    parent.GetType().Name);

            return cont;
        }
    }

}
