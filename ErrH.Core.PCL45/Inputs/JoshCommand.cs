using System;
using System.Diagnostics;
using System.Windows.Input;

namespace ErrH.Core.PCL45.Inputs
{
    public class JoshCommand : ILabeledCommand
    {
        readonly Action<object>    _execute;
        readonly Predicate<object> _canExecute;

        private      EventHandler _canExecuteChanged;
        public event EventHandler  CanExecuteChanged
        {
            add    { _canExecuteChanged -= value; _canExecuteChanged += value; }
            remove { _canExecuteChanged -= value; }
        }


        public JoshCommand(Action<object> execute, string label = null)
            : this(execute, null, label) { }


        public JoshCommand(Action<object> execute,
                           Predicate<object> canExecute,
                           string label = null)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute    = execute;
            _canExecute = canExecute;
            CurrentLabel = label;
        }


        public string  CurrentLabel  { get; set; }
        public bool    IsCheckable   { get; set; }
        public bool    IsChecked     { get; set; }

        public override string ToString() => CurrentLabel;

        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
            => _canExecute == null ? true : _canExecute(parameter);


        public void Execute(object parameter) => _execute(parameter);

    }
}
