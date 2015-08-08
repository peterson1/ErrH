using System;
using System.Windows.Automation;
using ErrH.Tools.WindowsAutomation.ItemShims;
using ErrH.WhiteShim.Extensions;
using TestStack.White.UIItems;

namespace ErrH.WhiteShim.ItemShims
{
    public class WhiteItemShim : UiItemShimBase
{
	private EventHandler _focus = delegate { };
	private IUIItem      _uiItem;


	internal WhiteItemShim(IUIItem uiItem)
	{
		this._uiItem = uiItem;
		this.CopyFrom(uiItem.AutomationElement);
	}
	


	public override bool CopyFrom(object automationElement)
	{
		var elm = automationElement as AutomationElement;
		if (elm == null) return false;

		this.Id = elm.Current.AutomationId;
		this.LocalType = elm.LocalType();
		this.Name = elm.Current.Name;
		this.AccessKey = elm.Current.AccessKey;

		return true;
	}


	public override event EventHandler Focus
	{
		add	{
			AutomationPropertyChangedEventHandler handlr = (s, e) 
				=> { _focus.Invoke(s, EventArgs.Empty); };

			Automation.AddAutomationPropertyChangedEventHandler
				(_uiItem.AutomationElement, TreeScope.Element, handlr,
				AutomationElement.HasKeyboardFocusProperty);

			_focus += value;
		}
		remove { _focus -= value; }
	}

}
}
