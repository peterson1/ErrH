namespace ErrH.Tools.WindowsAutomation.ElementDrivers
{
    public class ElementDriverBase
    {
        public string Key { get; set; }


        public ElementDriverBase(object key)
        {
            this.Key = key.ToString();
        }
    }
}
