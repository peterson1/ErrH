namespace ErrH.Tools.WindowsAutomation.ItemShims
{
    public interface IUiWindowShim
    {
        string Title { get; }
        string Text { get; }

        void Focus();
    }
}
