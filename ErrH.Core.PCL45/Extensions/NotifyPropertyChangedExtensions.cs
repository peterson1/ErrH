using System;
using System.ComponentModel;

namespace ErrH.Core.PCL45.Extensions
{
    public static class NotifyPropertyChangedExtensions
    {
        public static void OnChange(this INotifyPropertyChanged obj, string propertyName, Action action)
        {
            obj.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == propertyName) action?.Invoke();
            };
        }

    }
}
