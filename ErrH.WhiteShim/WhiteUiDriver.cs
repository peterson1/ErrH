using System;
using ErrH.Tools.Extensions;
using ErrH.Tools.Loggers;
using ErrH.Tools.WindowsAutomation.ElementDrivers;
using TestStack.White.UIItems;

namespace ErrH.WhiteShim
{
    public class WhiteUiDriver : LogSourceBase, IWindowUiDriver
{
	private WhiteElementFinder _find;


	internal WhiteUiDriver(WhiteElementFinder whiteFinder)
	{
		this._find = whiteFinder;
	}



	public bool Drive(WindowDriveRoute window)
	{
		Debug_n("Driving window elements...", "window title : “{0}”", _find.Window1.Title);
		foreach (dynamic step in window.Steps)
		{
			if (!this.Drive(step)) return false;
		}
		return Debug_n("Successfully driven all elements.", "Defined route has {0:step}.", window.Steps.Count);
	}



	public bool Drive(TextBoxDriver textBox)
	{
		return Drive(textBox, x => x.SetValue(textBox.Text),
				"TextBox({0}).SetValue(“{1}”)", textBox.Key, textBox.Text);
	}



	public bool Drive(ButtonDriver button)
	{
		if (!button.IsClicked) return Trace_n("  Button«{0}» was not clicked.", "");
		return Drive(button, x => x.Click(), 
				"Button({0}).Click()", button.Key);
	}


	private bool Drive(ElementDriverBase elementDrivr, Action<IUIItem> action,
		string taskIntro, params object[] msgArgs)
	{
		Trace_i("  " + taskIntro, msgArgs);
		var elm = _find.By(elementDrivr.Key);
		if (elm == null) return false;

		try {  action.Invoke(elm);  }
		catch (Exception ex) {
			return Error_o("Failed to invoke action on element." + L.F + ex.Details(false, false));	}

		return Trace_o("Successfully invoked action on element.");
	}
}
}
