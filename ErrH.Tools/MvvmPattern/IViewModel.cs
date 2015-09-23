using System.ComponentModel;
using ErrH.Tools.Loggers;

namespace ErrH.Tools.MvvmPattern
{
    //later: complete this
    public interface IViewModel : ILogSource, INotifyPropertyChanged
    {
        string DisplayName { get; }

    }
}
