using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrH.Tools.Loggers;
using ErrH.Tools.WindowsAutomation.ElementDrivers;

namespace ErrH.Tools.WindowsAutomation.ItemShims
{
    public interface IAppHijacker : ILogSource, IDisposable
    {
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
