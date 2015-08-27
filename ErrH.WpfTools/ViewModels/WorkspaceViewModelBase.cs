using System;
using System.ComponentModel;
using System.Windows.Input;
using ErrH.Tools.ErrorConstructors;
using ErrH.WpfTools.Commands;
using PropertyChanged;

namespace ErrH.WpfTools.ViewModels
{
    /// <summary>
    /// This ViewModelBase subclass requests to be removed 
    /// from the UI when its CloseCommand executes.
    /// This class is abstract.
    /// from MVVM Demo App : https://msdn.microsoft.com/en-us/magazine/dd419663.aspx
    /// </summary>
    [ImplementPropertyChanged]
    public abstract class WorkspaceViewModelBase : ViewModelBase, INotifyPropertyChanged
    {
        public event EventHandler RequestClose;
        public event EventHandler RequestRefresh;


        private int? _hashCode;


        public bool    IsBusy    { get; protected set; }
        public string  BusyText  { get; protected set; } = "Please wait ...";



        public ICommand CloseCommand => new RelayCommand(x => 
            RequestClose?.Invoke(this, EventArgs.Empty), x => !IsBusy);

        public ICommand RefreshCommand => new RelayCommand(x =>
            RequestRefresh?.Invoke(this, EventArgs.Empty), x => !IsBusy);




        public virtual void SetIdentifier(object identifier)
        {
            if (_hashCode.HasValue) Throw.BadAct("Identifier can only be set once.");
            _hashCode = HashCodeFor(identifier);
        }


        public virtual int HashCodeFor(object identifier) =>
            (GetType().Name + identifier.GetHashCode()).GetHashCode();


        public override int GetHashCode() 
            => _hashCode.GetValueOrDefault(0);






    }

}
