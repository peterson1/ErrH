using System;
using System.Windows.Input;
using ErrH.Tools.ErrorConstructors;
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

        private int? _hashCode;
        private RelayCommand _closeCommand;





        public virtual void SetIdentifier(object identifier)
        {
            if (_hashCode.HasValue) Throw.BadAct(
                "Identifier can only be set once.");

            _hashCode = HashCodeFor(identifier);
        }

        public virtual int HashCodeFor(object identifier)
        {
            var key = GetType().Name + identifier.GetHashCode();
            return key.GetHashCode();
        }

        public override int GetHashCode() => _hashCode.GetValueOrDefault(0);




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


    //public static class WorkspaceVM
    //{
    //    public static int HashCode<T>(object identifier)
    //        where T : WorkspaceViewModelBase
    //            => HashCode(typeof(T), identifier);


    //    public static int HashCode
    //        (Type vmType, object identifier)
    //    {
    //        var key = vmType.Name
    //                + (identifier?.ToString() ?? "");
    //        return key.GetHashCode();
    //    }
    //}
}
