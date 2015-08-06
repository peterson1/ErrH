using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ErrH.Tools.WindowsAutomation.ElementDrivers
{
    public abstract class WindowDriveRoute
    {
        private List<ElementDriverBase> _list;

        public ReadOnlyCollection<ElementDriverBase> Steps
        {
            get
            {
                if (_list == null)
                {
                    _list = new List<ElementDriverBase>();
                    this.DefineRoute();
                }
                return new ReadOnlyCollection<ElementDriverBase>(this._list);
            }
        }


        protected abstract void DefineRoute();


        protected TextBoxDriver TextBox(object automationIdOrText)
        {
            var drivr = new TextBoxDriver(automationIdOrText);
            this._list.Add(drivr);
            return drivr;
        }


        protected ButtonDriver Button(object automationIdOrText)
        {
            var drivr = new ButtonDriver(automationIdOrText);
            this._list.Add(drivr);
            return drivr;
        }
    }
}
