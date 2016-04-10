using System;
using System.ComponentModel;
using System.Threading.Tasks;
using ErrH.Core.PCL45.Extensions;
using PropertyChanged;

namespace ErrH.Core.PCL45.Inputs
{
    [ImplementPropertyChanged]
    public abstract class ClearyAsyncCmdBase : IAsyncCommand
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private      EventHandler _canExecuteChanged;
        public event EventHandler  CanExecuteChanged
        {
            add    { AddHandlerToCanExecuteChanged      (value); }
            remove { RemoveHandlerFromCanExecuteChanged (value); }
        }


        public string    CurrentLabel      { get; protected set; }
        public string    IdleLabel         { get; protected set; }
        public string    ExecutingLabel    { get; protected set; }

        public bool      IsRunning         { get; private set; }
        public bool      IsEnabled         { get; set; } = true;

        public abstract bool   CanExecute    (object parameter);
        public abstract Task   ExecuteAsync  (object parameter);


        protected virtual void AddHandlerToCanExecuteChanged(EventHandler handlr)
        {
            _canExecuteChanged -= handlr;
            _canExecuteChanged += handlr;
        }

        protected virtual void RemoveHandlerFromCanExecuteChanged(EventHandler handlr)
        {
            _canExecuteChanged -= handlr;
        }


        public async void Execute(object parameter)
        {
            IsRunning    = true;
            CurrentLabel = ExecutingLabel;
            await ExecuteAsync(parameter);
            CurrentLabel = IdleLabel;
            IsRunning    = false;
        }

        protected virtual void RaiseCanExecuteChanged()
            => _canExecuteChanged.Raise();
    }
}
