namespace ErrH.Tools.WindowsAutomation.ElementDrivers
{
    public class TextBoxDriver : ElementDriverBase
    {
        public string Text { get; set; }


        public TextBoxDriver(object key) : base(key) { }
    }
}
