using System;
using System.Diagnostics;
using System.Windows.Input;

namespace ErrH.Core.PCL45.Inputs
{
    public class JoshCommand : ICommand
    {
        readonly Action<object>    _execute;
        readonly Predicate<object> _canExecute;

        private      EventHandler _canExecuteChanged;
        public event EventHandler  CanExecuteChanged
        {
            add    { _canExecuteChanged -= value; _canExecuteChanged += value; }
            remove { _canExecuteChanged -= value; }
        }


        public JoshCommand(Action<object> execute)
            : this(execute, null) { }


        public JoshCommand(Action<object> execute,
                           Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute    = execute;
            _canExecute = canExecute;
        }


        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
            => _canExecute == null ? true : _canExecute(parameter);


        public void Execute(object parameter) => _execute(parameter);

    }
}
