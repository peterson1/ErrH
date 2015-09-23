using ErrH.Tools.WindowsAutomation.ItemShims;
using TestStack.White.UIItems;
using TestStack.White.UIItems.WindowItems;

namespace ErrH.WhiteShim.ItemShims
{
    internal class WhiteWindowShim : IUiWindowShim
{
    private Window _white;

    internal WhiteWindowShim(Window whiteWindow)
    {
        this._white = whiteWindow;
    }

        public string Title	=> _white.Title;
        public string Text  => _white.Get<Label>().Text;


        public void Focus()
    {
        _white.DisplayState = DisplayState.Minimized;
        _white.Focus(DisplayState.Restored);
    }
}
}
