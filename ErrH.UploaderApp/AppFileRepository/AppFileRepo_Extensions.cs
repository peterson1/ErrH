using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.FileSystemShims;

namespace ErrH.UploaderApp.AppFileRepository
{
    public static class AppFileRepo_Extensions
{

	internal static ReadOnlyCollection<AppFileItem> Files(this AppItem app)	{
		return app.Repo.AppFiles(app.Nid); }


	public static AppFileItem File(this AppItem app, int appFileNid) {
		return app.Repo.AppFileByNid(appFileNid); }


	//public static async Task<bool> Edit
	//	(this AppFileRepo_App app, string fieldName, object newValue) {
	//		return await app.Repo.Writer.UpdateAppNode(app, fieldName, newValue); }



	/// <summary>
	/// Creates a new App record, and saves it to data store.
	/// </summary>
	/// <returns></returns>
	public static async Task<bool> Add(this ReadOnlyCollection<AppItem> list,
		string title)
	{
		var lockdL = list as ChildList<AppItem, AppFileRepo>;
		if (lockdL == null) return false;

		return await lockdL.Parent.Writer.CreateAppNode(title);
	}




	public static async Task<AppFileItem> ReplaceFile
		(this AppFileItem appF, FileShim replacement) {
			return await appF.App.Repo.Writer.ReplaceFile(appF, replacement); }

	public static async Task<bool> TryReplaceFile(this AppFileItem appF, FileShim replacement) {
		return ((await appF.ReplaceFile(replacement)) != null); }



	public static async Task<AppFileItem> Add(this ReadOnlyCollection<AppFileItem> list, FileShim fsFile)
	{
		var lockdL = list as ChildList<AppFileItem, AppItem>;
		if (lockdL == null) return null;
		var app = lockdL.Parent;
		return await app.Repo.Writer.CreateAppFileNode(fsFile, app);
	}

	public static async Task<bool> TryAdd(this ReadOnlyCollection<AppFileItem> list, FileShim fsFile) {
		return ((await list.Add(fsFile)) != null); }




	public static async Task<FileShim> DownloadTo(this AppFileItem appF, FolderShim foldr) {
		return await appF.App.Repo.Downloader.Download(appF, foldr); }


	public static async Task<bool> TryDownloadTo(this AppFileItem appF, FolderShim foldr)
	{
		var ret = await appF.DownloadTo(foldr);
		return ret != null;
	}

	public static async Task<bool> Delete(this AppFileItem appF) {
		return await appF.App.Repo.Writer.DeleteAppFileNode(appF); }


}
}
