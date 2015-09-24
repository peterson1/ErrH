using System;
using System.Threading.Tasks;
using ErrH.Tools.Loggers;
using ErrH.Tools.ScalarEventArgs;
using ErrH.Tools.WindowsAutomation.ElementDrivers;

namespace ErrH.Tools.WindowsAutomation.ItemShims
{
    public interface IAppHijacker : ILogSource, IDisposable
    {
        event EventHandler<EArg<IUiWindowShim>> WindowOpened;

        IElementFinder Find { get; }
        IWindowUiDriver Driver { get; }
        int WindowCount { get; }

        bool AttachOrLaunch(string applicationExePath);

        Task WaitForWindowWithButton(string buttonText, int minutesTimeOut = 60);
        Task WaitForWindowClose(int minutesTimeOut = 60);
        Task WaitForAWindow(int minutesTimeOut = 60);

        Task<bool> Drive(WindowDriveRoute window);

        void DebugTextBoxes();


    }
}
