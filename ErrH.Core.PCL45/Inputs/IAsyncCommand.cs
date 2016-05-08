using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ErrH.Core.PCL45.Inputs
{
    public interface IAsyncCommand : ILabeledCommand, INotifyPropertyChanged
    {
        Task     ExecuteAsync   (object parameter);

        string   IdleLabel        { get; }
        string   ExecutingLabel   { get; }
        string   FinishedLabel    { get; set; }
        string   ErrorMessage     { get; }
        string   ErrorDetails     { get; }

        bool     IsRunning        { get; }
        bool     IsEnabled        { get; set; }
        bool     DisableAfterRun  { get; set; }
    }
}
