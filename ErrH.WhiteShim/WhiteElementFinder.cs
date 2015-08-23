using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation;
using ErrH.Tools.Extensions;
using ErrH.Tools.Loggers;
using ErrH.Tools.WindowsAutomation.ItemShims;
using ErrH.WhiteShim.ItemShims;
using TestStack.White;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace ErrH.WhiteShim
{

    internal class WhiteElementFinder : LogSourceBase, IElementFinder
{
	private Application _app;


	public WhiteElementFinder(Application whiteApp)
	{
		this._app = whiteApp;
	}


	public List<UiItemShimBase> TextBoxes {	get 
	{
		var list = new List<IUIItem>();

		list.AddRange(By(ControlType.Edit));
		list.AddRange(By(ControlType.Text));
		list.AddRange(By(ControlType.Document));

		return list.Select(x => new TextBoxShim(x) 
					as UiItemShimBase).ToList();
	}}







	internal List<Window> AppWindows { get { 
		return _app.GetWindows(); } }


	internal Window Window1 { get
	{
		var win = this.AppWindows.FirstOrDefault();
		if (win == null) Error_n("Failed to get Window1.", "Application has no open windows.");
		return win;
	}}


	internal List<IUIItem> By(ControlType typ) {
		return this.Window1.GetMultiple(
			SearchCriteria.ByControlType(typ)).ToList(); }



	internal IUIItem By(string automationIdOrText)
	{
		var sc = SearchCriteria.ByAutomationId(automationIdOrText);
		if (this.Window1.Exists(sc))
			return this.Window1.Get(sc);

		sc = SearchCriteria.ByText(automationIdOrText);
		if (this.Window1.Exists(sc))
			return this.Window1.Get(sc);

		Error_n("Element not found.", "using usable id: “{0}”.", automationIdOrText);
		return null;
	}


	public IUiWindowShim Window { get { 
		return new WhiteWindowShim(this.Window1); }}


	public List<UiItemShimBase> Others { get {
		return By(ControlType.Custom).Select(x => 
			new WhiteItemShim(x) as UiItemShimBase).ToList(); }}


	public List<IUiWindowShim> Windows { get {
		return AppWindows.Select(x => new WhiteWindowShim(x) 
								as IUiWindowShim).ToList(); }}






	public UiItemShimBase Button(string text)
	{
		Trace_i("Finding ‹Button› “{0}”...", text);
		Button b; try
		{	        
			b = Window1.Get<Button>(SearchCriteria.ByText(text));
		}
		catch (Exception ex) {
			return Error_o_<UiItemShimBase>(null, "‹Button› not found." + L.F + ex.Details(false, false)); }
		
		if (b == null) return Warn_o_<UiItemShimBase>(null, "‹Button› not found.");

		var shim = new WhiteItemShim(b);
		shim.Text = text;
		return Trace_o_(shim, "‹Button› found. [id: {0}]", shim.Id);
	}
}

}
