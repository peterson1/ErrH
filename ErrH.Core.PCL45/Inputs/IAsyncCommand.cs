using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ErrH.Core.PCL45.Inputs
{
    public interface IAsyncCommand : ICommand, INotifyPropertyChanged
    {
        Task     ExecuteAsync   (object parameter);

        string   CurrentLabel    { get; }
        string   IdleLabel       { get; }
        string   ExecutingLabel  { get; }

        bool     IsRunning       { get; }
        bool     IsEnabled       { get; set; }
    }
}
