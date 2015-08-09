using System;
using System.Windows.Input;

namespace ErrH.WpfTools.ViewModels
{
    /// <summary>
    /// Represents an actionable item displayed by a View.
    /// from MVVM Demo app : https://msdn.microsoft.com/en-us/magazine/dd419663.aspx
    /// </summary>
    public class CommandViewModel : ViewModelBase
    {
        public CommandViewModel(string displayName, ICommand command)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            base.DisplayName = displayName;
            this.Command = command;
        }

        public ICommand Command { get; private set; }
    }
}
