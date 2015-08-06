using System.Linq;
using System.Threading.Tasks;
using ErrH.Tools.Extensions;
using ErrH.Tools.Loggers;
using ErrH.Tools.WindowsAutomation.ElementDrivers;

namespace ErrH.Tools.WindowsAutomation.ItemShims
{
    public abstract class AppHijackerBase : LogSourceBase, IAppHijacker
    {

        public IElementFinder Find { get; protected set; }
        public IWindowUiDriver Driver { get; protected set; }

        public abstract bool AttachOrLaunch(string applicationExePath);

        public abstract Task WaitForWindowWithButton(string buttonText, int minutesTimeOut);
        public abstract Task WaitForWindowClose(int minutesTimeOut);
        public abstract Task WaitForAWindow(int minutesTimeOut);

        public abstract Task<bool> Drive(WindowDriveRoute window);


        public int WindowCount
        {
            get
            {
                return Find.Windows.Count;
            }
        }


        public void DebugTextBoxes()
        {
            Debug_h("Debugging all textboxes in current window...", "");
            foreach (var tbox in Find.TextBoxes)
            {
                var titl = "{0} [id: {1}] “{2}”"
                            .f(tbox.LocalType.Guillemet().PadLeft(12),
                               tbox.Id.PadLeft(2), tbox.Name);

                Debug_n(titl, "“{0}”", tbox.Text);
            }
        }


        public abstract void Dispose();
    }
}
