using System;
using System.Linq;
using ErrH.Tools.MvcPattern;
using ErrH.Tools.ScalarEventArgs;

namespace ErrH.UploaderApp.MvcPattern
{

    public partial class UploaderFormController : MvcControllerBase
{
	public event EventHandler<EnableEventArg>  UploadEnabled    = delegate { };
	public event EventHandler<EnableEventArg>  AppListEnabled   = delegate { };
	public event EventHandler<EnableEventArg>  MenuItemsEnabled = delegate { };
	
	
	public override void ToggleElements(CtrlState state)
	{
		UploadEnabled    (this, EArg(state, EnableUpload));
		AppListEnabled   (this, EArg(state, true));
		MenuItemsEnabled (this, EArg(state, true));
	}


	private bool EnableUpload { get
	{
		var files = View.ListedFiles;
		if (files == null) return false;
		if (files.Count == 0) return false;
		if (!files.All(x =>	
			x.Compared != VsRemote.Unknown)) return false;
		if (files.All(x =>
			x.Compared == VsRemote.Same)) return false;

		return true;
	}}	
}


}
