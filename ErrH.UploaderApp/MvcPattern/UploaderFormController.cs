using System;
using System.Threading.Tasks;
using ErrH.Tools.MvcPattern;
using ErrH.UploaderApp.EventArguments;
using ErrH.UploaderApp.Models;

namespace ErrH.UploaderApp.MvcPattern
{
    public partial class UploaderFormController : MvcControllerBase
{
	public event EventHandler<AppDirEventArgs>  AppsListChanged  = delegate { };
	public event EventHandler<AppFileEventArgs> FilesListChanged = delegate { };

	public IUploaderWindow   View  { get; set; }
	public UploaderFormData  Data  { get; set; }



	async void View_ReplaceLocalsClicked(object sender, AppDirEventArgs e)
	{
		using (var t = new FormToggle(this))
			await Data.ReplaceLocals(e.App);

		await RefreshFilesGrid(e.App);
	}


	async void View_UploadClicked(object sender, AppFileEventArgs e)
	{
		using (var t = new FormToggle(this))
		{
			await Data.UploadChangedFiles(e.App, e.List);
			await RefreshFilesGrid(e.App);
		}
	}

	

	async void View_AppSelected(object sender, AppDirEventArgs e)
	{
		await RefreshFilesGrid(e.App);
	}


	private async Task RefreshFilesGrid(AppDir app)
	{
		using (var t = new FormToggle(this))
		{
			var files = await Data.FilesForApp(app);
			if (files == null) return;
			FilesListChanged(this, EvtArg.AppFile(files));
			await Data.CompareAgainstRemote(files, app);
			View.SortFiles("Compared");
		}
	}


	public async override Task OnViewLoad()
	{
		await TaskEx.Delay(0);

		View.AppSelected          += View_AppSelected;
		View.UploadClicked        += View_UploadClicked;
		View.ReplaceLocalsClicked += View_ReplaceLocalsClicked;

		var list = Data.GetAppsList(this.AppFolder);
		if (list == null) return;
		AppsListChanged(this, EvtArg.AppDir(list));			
	}



	public override void OnViewUnload()
	{
		if (Data != null) Data.Dispose();
	}
}
}
