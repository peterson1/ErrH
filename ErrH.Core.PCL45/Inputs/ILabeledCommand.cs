using System.Windows.Input;

namespace ErrH.Core.PCL45.Inputs
{
    public interface ILabeledCommand : ICommand
    {
        string  CurrentLabel  { get; set; }
    }
}
