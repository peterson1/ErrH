using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ErrH.Tools.Extensions;

namespace ErrH.Wpf.net45.Commands
{
    public class TrappedCommand : ICommand
    {
        private readonly Action<object>    _action;
        private readonly Predicate<object> _canExecute;

        public event EventHandler CanExecuteChanged
        {
            add    { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }



        public TrappedCommand(Action<object> action,
                              Predicate<object> canExecute = null)
        {
            if (action == null) throw new ArgumentNullException("execute");
            _action     = action;
            _canExecute = canExecute;
        }



        public void Execute(object parameter)
        {
            try {  BeforeExecute(parameter);  }
            catch (Exception ex) { OnError(ex, "Error Before Execute"); }

            try {  _action(parameter);  }
            catch (Exception ex) { OnError(ex, "Error On Execute"); }

            try {  AfterExecute(parameter);  }
            catch (Exception ex) { OnError(ex, "Error After Execute"); }
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
    }
}
