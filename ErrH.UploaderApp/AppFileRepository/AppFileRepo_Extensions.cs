using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.FileSystemShims;

namespace ErrH.UploaderApp.AppFileRepository
{
    public static class AppFileRepo_Extensions
{

	internal static ReadOnlyCollection<AppFileNode> Files(this AppNode app)	{
		return app.Repo.AppFiles(app.Nid); }


	public static AppFileNode File(this AppNode app, int appFileNid) {
		return app.Repo.AppFileByNid(appFileNid); }


	//public static async Task<bool> Edit
	//	(this AppFileRepo_App app, string fieldName, object newValue) {
	//		return await app.Repo.Writer.UpdateAppNode(app, fieldName, newValue); }



	/// <summary>
	/// Creates a new App record, and saves it to data store.
	/// </summary>
	/// <returns></returns>
	public static async Task<bool> Add(this ReadOnlyCollection<AppNode> list,
		string title)
	{
		var lockdL = list as ChildList<AppNode, AppFileRepo>;
		if (lockdL == null) return false;

		return await lockdL.Parent.Writer.CreateAppNode(title);
	}




	public static async Task<AppFileNode> ReplaceFile
		(this AppFileNode appF, FileShim replacement) {
			return await appF.App.Repo.Writer.ReplaceFile(appF, replacement); }

	public static async Task<bool> TryReplaceFile(this AppFileNode appF, FileShim replacement) {
		return ((await appF.ReplaceFile(replacement)) != null); }



	public static async Task<AppFileNode> Add(this ReadOnlyCollection<AppFileNode> list, FileShim fsFile)
	{
		var lockdL = list as ChildList<AppFileNode, AppNode>;
		if (lockdL == null) return null;
		var app = lockdL.Parent;
		return await app.Repo.Writer.CreateAppFileNode(fsFile, app);
	}

	public static async Task<bool> TryAdd(this ReadOnlyCollection<AppFileNode> list, FileShim fsFile) {
		return ((await list.Add(fsFile)) != null); }




	public static async Task<FileShim> DownloadTo(this AppFileNode appF, FolderShim foldr) {
		return await appF.App.Repo.Downloader.Download(appF, foldr); }


	public static async Task<bool> TryDownloadTo(this AppFileNode appF, FolderShim foldr)
	{
		var ret = await appF.DownloadTo(foldr);
		return ret != null;
	}

	public static async Task<bool> Delete(this AppFileNode appF) {
		return await appF.App.Repo.Writer.DeleteAppFileNode(appF); }


}
}
