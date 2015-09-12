using System;
using System.Windows.Input;
using ErrH.Tools.ErrorConstructors;
using ErrH.WpfTools.Commands;

namespace ErrH.WpfTools.ViewModels
{
    /// <summary>
    /// Represents an actionable item displayed by a View.
    /// from MVVM Demo app : https://msdn.microsoft.com/en-us/magazine/dd419663.aspx
    /// </summary>
    public class CommandVM : ViewModelBase
    {

        public ICommand Command { get; private set; }



        public CommandVM(string displayName, 
            Action<object> execute, Predicate<object> canExecute = null)
                : this(displayName, new RelayCommand(execute, canExecute)) { }


        public CommandVM(string displayName, ICommand command)
        {
            Throw.IfNull(command, nameof(command));
            base.DisplayName = displayName;
            this.Command = command;
        }

    }
}
