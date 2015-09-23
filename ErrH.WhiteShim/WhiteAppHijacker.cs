using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Automation;
using ErrH.Tools.Extensions;
using ErrH.Tools.WindowsAutomation.ElementDrivers;
using ErrH.Tools.WindowsAutomation.ItemShims;
using TestStack.White;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;

namespace ErrH.WhiteShim
{

    public class WhiteAppHijacker : AppHijackerBase, IDisposable
    {
        private Application _app;
        internal WhiteElementFinder _find;



        public override async Task<bool> Drive(WindowDriveRoute window)
        {
            return await Task.Run(() =>
            {
                return this.Driver.Drive(window);
            });
        }


        public override bool AttachOrLaunch(string applicationExePath)
        {
            Info_n("Starting App Watcher...", "for: " + applicationExePath);

            try
            {
                var proc = new ProcessStartInfo(applicationExePath);
                _app = Application.AttachOrLaunch(proc);
            }
            catch (Exception ex) {
                return Error_n("Failed to start app watcher.", ex.Details(true, false)); }

            this._find = ForwardLogs(new WhiteElementFinder(_app));
            this.Find = this._find;
            this.Driver = ForwardLogs(new WhiteUiDriver(_find));

            return true;
        }


        public override async Task WaitForAWindow(int minutesTimeOut)
        {
            Debug_n("Waiting for a window to open...", "application: " + _app.Name);

            while (this.WindowCount == 0)
                await WaitFor.Event(WindowPattern.WindowOpenedEvent);

            Debug_n("A window has opened.", "Hello window “{0}”!", Find.Window.Title);

            //_find.Window1.MessageBox("").Get<Label>().te
            //Find.Window.Title
        }


        public override async Task WaitForWindowClose(int minutesTimeOut)
        {
            var win = _find.Window1;

            Debug_n("Waiting for window to close...", $"title: “{win.Title}”");

            await Task.Run(() =>
            {
                win.WaitTill(() => win.IsClosed, TimeSpan.FromMinutes(minutesTimeOut));
            });

            Debug_n("Window has been closed.", "Goodbye window “{win.Title}”!");
        }


        public override async Task WaitForWindowWithButton(string buttonText, int minutesTimeOut)
        {
            Info_n("Waiting for a specific window...", $"containing a button with text: “{buttonText}”");

            var crit = SearchCriteria.ByText(buttonText);

            while (this.WindowCount == 0 || !_find.Window1.Exists<Button>(crit))
            {
                if (this.WindowCount == 0)
                {
                    Debug_n("Waiting for a specific window...", "Application has no open windows.");
                    await this.WaitForAWindow(minutesTimeOut);
                }
                else
                {
                    Debug_n("Waiting for a specific window...", "Button “{0}” not in window “{1}”.", buttonText, Find.Window.Title);
                    await this.WaitForWindowClose(minutesTimeOut);
                }
            }

            Info_n("Specific window found.", "Button “{0}” found in window “{1}”.", buttonText, Find.Window.Title);
        }




        public override void Dispose()
        {
            //this.WaitForFocus.Dispose();
            Automation.RemoveAllEventHandlers();
            if (_app != null) _app.Dispose();
        }
    }
}
