using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ErrH.Tools.Extensions;

namespace ErrH.Wpf.net45.Commands
{
    public class ResultingCommand<T> : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add    { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        private readonly Func<object, Task<T>> _function;
        protected Predicate<object>  _canExecute;


        public T Result { get; private set; }


        public ResultingCommand(Func<object, Task<T>> function, Predicate<object> canExecute = null)
        {
            if (function == null) throw new ArgumentNullException("function");
            _function   = function;
            _canExecute = canExecute;
        }


        public virtual async void Execute(object parameter)
        {
            try {  BeforeExecute(parameter);  }
            catch (Exception ex) { OnError(ex, "Error Before Execute"); }

            try {
                await Task.Run(async () =>
                {
                    Result = await _function(parameter);
                });
            }
            catch (Exception ex) { OnError(ex, "Error On Execute"); }

            try {  AfterExecute(parameter);  }
            catch (Exception ex) { OnError(ex, "Error After Execute"); }

            RaiseCanExecuteChanged();
        }




        protected virtual void BeforeExecute(object parameter){}
        protected virtual void AfterExecute (object parameter){}


        protected virtual void OnError(Exception ex, string caption = null)
        {
            if (caption.IsBlank()) caption = ex.Message;
            MessageBox.Show(ex.Details(false, false), caption, 
                MessageBoxButton.OK, MessageBoxImage.Error);
        }


        public bool CanExecute(object parameter)
            => _canExecute?.Invoke(parameter) ?? true;


        private void RaiseCanExecuteChanged()
            => CommandManager.InvalidateRequerySuggested();
    }
}
