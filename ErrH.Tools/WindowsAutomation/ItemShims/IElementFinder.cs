using System.Collections.Generic;
using ErrH.Tools.Loggers;

namespace ErrH.Tools.WindowsAutomation.ItemShims
{
    public interface IElementFinder : ILogSource
    {
        List<IUiWindowShim> Windows { get; }
        IUiWindowShim Window { get; }

        List<UiItemShimBase> TextBoxes { get; }
        List<UiItemShimBase> Others { get; }

        UiItemShimBase Button(string text);
    }
}
