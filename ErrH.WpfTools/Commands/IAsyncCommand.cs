using System.Threading.Tasks;
using System.Windows.Input;

namespace ErrH.WpfTools.Commands
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object parameter);
    }
}
