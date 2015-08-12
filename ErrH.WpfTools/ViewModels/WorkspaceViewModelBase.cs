using System;
using System.Windows.Input;
using ErrH.WpfTools.Commands;

namespace ErrH.WpfTools.ViewModels
{
    /// <summary>
    /// This ViewModelBase subclass requests to be removed 
    /// from the UI when its CloseCommand executes.
    /// This class is abstract.
    /// from MVVM Demo App : https://msdn.microsoft.com/en-us/magazine/dd419663.aspx
    /// </summary>
    public abstract class WorkspaceViewModelBase : ViewModelBase
    {

        private RelayCommand _closeCommand;


        protected WorkspaceViewModelBase()
        {
        }



        #region CloseCommand

        /// <summary>
        /// Returns the command that, when invoked, attempts
        /// to remove this workspace from the user interface.
        /// </summary>
        public ICommand CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                    _closeCommand = new RelayCommand(param => this.OnRequestClose());

                return _closeCommand;
            }
        }

        #endregion // CloseCommand




        #region RequestClose [event]

        /// <summary>
        /// Raised when this workspace should be removed from the UI.
        /// </summary>
        public event EventHandler RequestClose;

        void OnRequestClose()
        {
            EventHandler handler = this.RequestClose;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        #endregion // RequestClose [event]
    }
}
