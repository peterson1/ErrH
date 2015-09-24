using ErrH.Tools.WindowsAutomation.ItemShims;
using TestStack.White.UIItems;
using TestStack.White.UIItems.WindowItems;

namespace ErrH.WhiteShim.ItemShims
{
    internal class WhiteWindowShim : IUiWindowShim
    {
        private Window _white;
        private string _text = "";

        internal WhiteWindowShim(Window whiteWindow)
        {
            this._white = whiteWindow;
            try
            {
                if (_white.Exists<Label>())
                    _text = _white.Get<Label>().Text;
            }
            catch { }
        }

        public string Title	=> _white.Title;
        public string Text => _text;

        public void Focus()
        {
            _white.DisplayState = DisplayState.Minimized;
            _white.Focus(DisplayState.Restored);
        }
}
}
