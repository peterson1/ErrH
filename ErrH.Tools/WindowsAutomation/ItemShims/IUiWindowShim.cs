namespace ErrH.Tools.WindowsAutomation.ItemShims
{
    public interface IUiWindowShim
    {
        string Title { get; }

        void Focus();
    }
}
