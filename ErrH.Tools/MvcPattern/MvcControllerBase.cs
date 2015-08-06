using System;
using System.Threading.Tasks;
using ErrH.Tools.Loggers;
using ErrH.Tools.ScalarEventArgs;

namespace ErrH.Tools.MvcPattern
{
    public enum CtrlState { Enabled, Disabled, Conditional }


    public abstract class MvcControllerBase : LogSourceBase
    {
        public string AppFolder;

        public abstract Task OnViewLoad();
        public abstract void OnViewUnload();

        public abstract void ToggleElements(CtrlState state);


        protected EnableEventArg EArg(CtrlState state, bool condition)
        {
            if (state == CtrlState.Enabled) return EventArg.Enable(true);
            if (state == CtrlState.Disabled) return EventArg.Enable(false);
            return EventArg.Enable(condition);
        }

    }


    public class FormToggle : IDisposable
    {
        private MvcControllerBase _ctrlr;


        public FormToggle(MvcControllerBase controller)
        {
            this._ctrlr = controller;
            this._ctrlr.ToggleElements(CtrlState.Disabled);
        }


        public void Dispose()
        {
            this._ctrlr.ToggleElements(CtrlState.Conditional);
        }
    }
}
