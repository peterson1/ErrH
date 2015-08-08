using System.Windows.Automation;
using ErrH.Tools.Extensions;
using ErrH.WhiteShim.Extensions;
using TestStack.White.UIItems;

namespace ErrH.WhiteShim.ItemShims
{

    internal class TextBoxShim : WhiteItemShim
{

	internal TextBoxShim(IUIItem uiItem) : base(uiItem) { }

	public override bool CopyFrom(object automationElement)
	{
		if (!base.CopyFrom(automationElement)) return false;
		this.Text = ((AutomationElement)automationElement).GetText();
		return true;
	}


	public override string ToString()
	{
		return $"{LocalType.Guillemet().PadLeft(12)} [id: {Id.PadLeft(2)}] “{Name}” =  “{Text}”";
	}

}
}
