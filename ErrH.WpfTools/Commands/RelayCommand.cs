using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using ErrH.Tools.Extensions;

namespace ErrH.WpfTools.Commands
{
    /// <summary>
    /// A command whose sole purpose is to 
    /// relay its functionality to other
    /// objects by invoking delegates. The
    /// default return value for the CanExecute
    /// method is 'true'.
    /// from MVVM Demo App: https://msdn.microsoft.com/en-us/magazine/dd419663.aspx
    /// </summary>
    public class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add    { CommandManager.RequerySuggested -= value; CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }


        readonly Action    <object>    _execute;
        readonly Predicate <object> _canExecute;




        /// <summary>
        /// Creates a new command that can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public RelayCommand(Action<object> execute) 
            : this(execute, null) { }


        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand(Action    <object>    execute, 
                            Predicate <object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute    = execute;
            _canExecute = canExecute;
        }




        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true 
                 : _canExecute(parameter);
        }


        public void Execute(object parameter)
        {
            try {
                _execute(parameter);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Details(true, false), 
                    $"Error in {GetType().Name}.Execute()");
            }
        }

    }
}
