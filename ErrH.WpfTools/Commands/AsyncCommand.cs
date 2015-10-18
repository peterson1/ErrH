using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ErrH.WpfTools.TaskShims;
using PropertyChanged;

namespace ErrH.WpfTools.Commands
{
    //from https://msdn.microsoft.com/en-us/magazine/dn630647.aspx

    [ImplementPropertyChanged]
    public class AsyncCommand<TResult> : AsyncCommandBase
    {
        private readonly Func<CancellationToken, Task<TResult>> _command;
        private readonly CancelAsyncCommand _cancelCommand;
        private readonly Predicate<object> _canExecute;


        public TalkyTask<TResult> Execution { get; private set; }

        public ICommand CancelCommand => _cancelCommand;


        public AsyncCommand(Func<CancellationToken, Task<TResult>> command)
        {
            _command = command;
            _cancelCommand = new CancelAsyncCommand();
        }

        public AsyncCommand(Func<CancellationToken, Task<TResult>> command,
                            Predicate<object> canExecute)
            : this(command)
        {
            _canExecute = canExecute;
        }


        public override bool CanExecute(object parameter)
        {
            if (_canExecute != null)
                if (!_canExecute(null)) return false;
            return Execution == null || Execution.IsCompleted;
        }


        public override async Task ExecuteAsync(object parameter)
        {
            _cancelCommand.NotifyCommandStarting();
            Execution = new TalkyTask<TResult>(_command(_cancelCommand.Token));
            RaiseCanExecuteChanged();
            await Execution.TaskCompletion;
            _cancelCommand.NotifyCommandFinished();
            RaiseCanExecuteChanged();
        }



        private sealed class CancelAsyncCommand : ICommand
        {
            public event EventHandler CanExecuteChanged
            {
                add    { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }


            private CancellationTokenSource _cts = new CancellationTokenSource();
            private bool _commandExecuting;


            public CancellationToken Token => _cts.Token;


            public void NotifyCommandStarting()
            {
                _commandExecuting = true;
                if (!_cts.IsCancellationRequested) return;
                _cts = new CancellationTokenSource();
                RaiseCanExecuteChanged();
            }


            public void NotifyCommandFinished()
            {
                _commandExecuting = false;
                RaiseCanExecuteChanged();
            }


            public bool CanExecute(object parameter)
                => _commandExecuting && !_cts.IsCancellationRequested;


            public void Execute(object parameter)
            {
                _cts.Cancel();
                RaiseCanExecuteChanged();
            }


            private void RaiseCanExecuteChanged()
                => CommandManager.InvalidateRequerySuggested();
        }


    }



    public static class AsyncCommand
    {
        public static AsyncCommand<object> Create(Func<Task> command)
            => new AsyncCommand<object>(async _ => { await command(); return null; });

        public static AsyncCommand<TResult> Create<TResult>(Func<Task<TResult>> command)
            => new AsyncCommand<TResult>(_ => command());

        public static AsyncCommand<object> Create(Func<CancellationToken, Task> command)
            => new AsyncCommand<object>(async token => { await command(token); return null; });

        public static AsyncCommand<TResult> Create<TResult>(Func<CancellationToken, Task<TResult>> command)
            => new AsyncCommand<TResult>(command);

        public static AsyncCommand<TResult> Create<TResult>
            (Func<CancellationToken, Task<TResult>> command,
                            Predicate<object> canExecute)
            => new AsyncCommand<TResult>(command, canExecute);
    }
}
