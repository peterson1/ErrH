using System.Windows.Input;

namespace ErrH.WpfTools.Extensions
{
    public static class ICommandExtensions
    {
        public static void ExecuteIfItCan(this ICommand cmd, object parameter = null)
        {
            if (cmd.CanExecute(parameter))
                cmd.Execute(parameter);
        }
    }
}
