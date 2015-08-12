using System.Linq;
using System.Threading.Tasks;
using ErrH.Tools.Drupal7Models;
using ErrH.Tools.Drupal7Models.Entities;
using ErrH.Tools.Drupal7Models.Fields;
using ErrH.Tools.FileSystemShims;
using ErrH.Tools.Loggers;
using ErrH.UploaderApp.DTOs;

namespace ErrH.UploaderApp.AppFileRepository
{
    internal class AppFileRepo_Writer : LogSourceBase
{
	private ID7Client    _client;
	private AppFileRepo  _repo;


	public AppFileRepo_Writer(ID7Client d7Client, AppFileRepo repo)
	{
		this._client = d7Client;
		this._repo   = repo;
	}


	internal async Task<bool> CreateAppNode(string title)
	{
		var usr = _client.CurrentUser;
		var d7n = await _client.Post(new AppDto {
			title       = title,
			field_users_ref = und.TargetIds(usr.uid)
		});
		if (!d7n.IsValidNode()) return false;

		return await _repo.RefreshData();
	}


	internal async Task<AppFileNode> CreateAppFileNode
		(FileShim fsFile, AppNode app)
	{
		//  reject if file with the same name already belongs to the app
		if (app.Files.Count(x => x.Name == fsFile.Name) != 0)
		{
			Warn_h("Attempted to upload conflicting file.", "File “{0}” already exists in app “{1}”.", fsFile.Name, app.Title);
			return null;
		}

		var fid = await UploadFile(fsFile);
		if (fid == -1) return null;

		var appF = await _client.Post(AppFileDto.From(fsFile, fid));
		if (!appF.IsValidNode()) return null;

		if (!(await AttachAppFileToApp(appF, app))) return null;

		if (!(await _repo.RefreshData(app.Nid))) return null;

		return _repo.AppFileByNid(appF.nid);
	}


	private async Task<int> UploadFile(FileShim fsFile)
	{
		return await _client.Post(fsFile, SERVER_DIR.app_files);
	}


	private async Task<bool> AttachAppFileToApp
		(ID7Node fileNode, AppNode app)
	{
		var fNids = app.Files.Select(x => x.Nid).ToList();
		fNids.Add(fileNode.nid);

		var dto = AppDtoRevision.From(app);
		dto.field_files_ref.und.Add(
			und.TargetId(fileNode.nid));

		var ret = await _client.Put(dto, "Updating ‹app› node to include ‹app-file›...", 
										 "Success. ‹app›[nid:{0}] now includes ‹app-file›[nid:{1}].", x => app.Nid, x => fileNode.nid);
		return ret.IsValidNode();
	}



	//internal async Task<bool> UpdateAppNode
	//	(AppFileRepo_App app, string fieldName, object newValue)
	//{
	//	return await _client.Put(app.Nid, Dict.onary(fieldName, newValue));
	//}


	internal async Task<bool> DeleteAppFileNode(AppFileNode node)
	{
		if (!(await DetachFileFromNode(node))) return false;

		var fList = node.App.Files.Select(x => x.Nid).ToList();
		fList.Remove(node.Nid);

		var appRev = AppDtoRevision.From(node.App);
		appRev.field_files_ref = und.TargetIds(fList.ToArray());

		var ret = await _client.Put(appRev, "Updating ‹app› node to exclude ‹app-file›...");
		if (!ret.IsValidNode()) return false;

		return await _repo.RefreshData(node.App.Nid);
	}



	/// <summary>
	/// this seems to delete the file from the server
	///   - so we don't need to manually request for deletion
	/// </summary>
	/// <param name="node"></param>
	/// <returns></returns>
	private async Task<bool> DetachFileFromNode(AppFileNode node)
	{
		node.Fid = -1;

		var ret = await _client.Put(AppFileDtoRevision.From(node),
			"Detaching file from node...",
			"File “{1}” successfully detached from node [nid: {0}].",
			x => node.Nid, x => node.Name);

		return ret.IsValidNode();
	}


	internal async Task<AppFileNode> ReplaceFile
		(AppFileNode node, FileShim replacement)
	{
		//  detach fid from node
		if (!(await DetachFileFromNode(node))) return null;

		//  upload new fid
		var newFid = await UploadFile(replacement);
		if (newFid == -1) return null;

		//  attach new fid to node
		node.Fid = newFid;
		var nodeRev = AppFileDtoRevision.From(node, replacement);

		//  save revised node
		var ret = await _client.Put(nodeRev,
						"Attaching uploaded file to node...", 
						"“{2}” [fid: {0}] successfully attached to [nid: {1}]",
						x => newFid, x => node.Nid, x => replacement.Name);
		
		if (!ret.IsValidNode()) return null;

		//  refresh data
		if (!(await _repo.RefreshData(node.App.Nid))) return null;

		//  return the node
		return _repo.AppFileByNid(ret.nid);
	}
}
}
