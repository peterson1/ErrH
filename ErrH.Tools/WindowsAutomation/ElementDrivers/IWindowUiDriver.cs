using ErrH.Tools.Loggers;

namespace ErrH.Tools.WindowsAutomation.ElementDrivers
{
    public interface IWindowUiDriver : ILogSource
    {
        bool Drive(WindowDriveRoute window);
        bool Drive(TextBoxDriver textBox);
        bool Drive(ButtonDriver button);
    }
}
