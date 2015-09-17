using System;
using System.Collections.Generic;
using System.ComponentModel;
using ErrH.Tools.Loggers;

namespace ErrH.Tools.MvvmPattern
{
    public abstract class ViewModelBase : LogSourceBase, INotifyPropertyChanged, IDisposable
    {
        private      PropertyChangedEventHandler _propertyChanged;
        public event PropertyChangedEventHandler  PropertyChanged
        {
            add    { _propertyChanged -= value; _propertyChanged += value; }
            remove { _propertyChanged -= value; }
        }


        private string _displayName;
        public virtual string DisplayName
        {
            get { return _displayName; }
            protected set { SetField(ref _displayName, value, nameof(DisplayName)); }
        }



        public override string ToString() => DisplayName;



        protected void RaisePropertyChanged(string propertyName)
            => _propertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));



        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            RaisePropertyChanged(propertyName);
            return true;
        }



        protected virtual void OnDispose()
        {
        }


        public void Dispose()
        {
            this.OnDispose();
        }
    }
}
