using System;
using System.Diagnostics;
using System.Windows.Input;

namespace ErrH.Wpf.net45.Commands
{
    public class JoshCommand : ICommand
    {
        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;


        public JoshCommand(Action<object> execute)
            : this(execute, null)
        { }


        public JoshCommand(Action<object> execute,
                            Predicate<object> canExecute)
        {
            if (execute == null) throw new ArgumentNullException("execute");
            _execute = execute;
            _canExecute = canExecute;
        }


        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
            => _canExecute == null ? true : _canExecute(parameter);


        public event EventHandler CanExecuteChanged
        {
            add    { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter) => _execute(parameter);

    }
}
