using System;
using System.ComponentModel;
using System.Windows.Input;
using ErrH.Tools.ErrorConstructors;
using ErrH.WpfTools.Commands;
using ErrH.WpfTools.Extensions;
using PropertyChanged;

namespace ErrH.WpfTools.ViewModels
{
    [ImplementPropertyChanged]
    public abstract class WorkspaceViewModelBase : ViewModelBase, INotifyPropertyChanged
    {
        private EventHandler _closed;
        private EventHandler _refreshed;

        public event EventHandler Closed
        {
            add    { _closed -= value; _closed += value; }
            remove { _closed -= value; }
        }
        public event EventHandler Refreshed
        {
            add    { _refreshed -= value; _refreshed += value; }
            remove { _refreshed -= value; }
        }


        private int? _hashCode;


        public bool    IsBusy    { get; protected set; }
        public string  BusyText  { get; protected set; } = "Please wait ...";



        public void Close   () => CloseCommand  .ExecuteIfItCan();
        public void Refresh () => RefreshCommand.ExecuteIfItCan();


        private ICommand _closeCommand;
        public  ICommand  CloseCommand
        {
            get
            {
                if (_closeCommand != null) return _closeCommand;
                _closeCommand = new RelayCommand(
                    x => _closed?.Invoke(this, EventArgs.Empty), 
                    x => !IsBusy);
                return _closeCommand;
            }
        }

        private ICommand _refreshCommand;
        public  ICommand  RefreshCommand
        {
            get
            {
                if (_refreshCommand != null) return _refreshCommand;
                _refreshCommand = new RelayCommand(
                    x => _refreshed?.Invoke(this, EventArgs.Empty), 
                    x => !IsBusy);
                return _refreshCommand;
            }
        }

        





        public virtual void SetIdentifier(object identifier)
        {
            if (_hashCode.HasValue) Throw.BadAct("Identifier can only be set once.");
            _hashCode = HashCodeFor(identifier);
        }


        public virtual int HashCodeFor(object identifier) =>
            (GetType().Name + identifier.GetHashCode()).GetHashCode();


        public override int GetHashCode() 
            => _hashCode.GetValueOrDefault(0);






    }

}
