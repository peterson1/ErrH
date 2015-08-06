namespace ErrH.Tools.WindowsAutomation.ElementDrivers
{
    public class ButtonDriver : ElementDriverBase
    {
        public bool IsClicked { get; private set; }

        public void Click()
        {
            this.IsClicked = true;
        }


        public ButtonDriver(object key) : base(key) { }
    }
}
