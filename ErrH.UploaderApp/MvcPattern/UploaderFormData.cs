using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ErrH.Tools.Drupal7Models;
using ErrH.Tools.Extensions;
using ErrH.Tools.FileSystemShims;
using ErrH.Tools.Loggers;
using ErrH.UploaderApp.AppFileRepository;
using ErrH.UploaderApp.DTOs;
using ErrH.UploaderApp.Models;
using ErrH.UploaderApp.Services;

namespace ErrH.UploaderApp.MvcPattern
{
    public class UploaderFormData : LogSourceBase, IDisposable
{
	public UploaderCfgFile ConfigFile;
	private IFileSystemShim _fs;
	private AppFileRepo     _repo;
	private ID7Client       _client;

	public UploaderFormData(UploaderCfgFile configFile, 
							IFileSystemShim fsShim,
							ID7Client d7Client,
							AppFileRepo appFileRepo)
	{
		this.ConfigFile = ForwardLogs(configFile);
		this._fs        = ForwardLogs(fsShim);
		this._client    = ForwardLogs(d7Client);
		this._repo      = ForwardLogs(appFileRepo);
	}



	public List<AppDir> GetAppsList(string startupDirPath)
	{
		if (!ConfigFile.ReadFrom<UploaderCfgFileDto>(startupDirPath)) return null;
		var list = ConfigFile.LocalApps;
		return Debug_(list, "Getting list of apps from config file...", "{0:app} found.", list.Count);
	}



	public async Task<List<AppFile>> FilesForApp(AppDir appDir)
	{
		var files = ConfigFile.FindFiles(appDir);

		if (!_client.IsLoggedIn)
		{
			_client.LoadSession();
            
			var usr = ConfigFile.AppUser;
			if (!(await _repo.Connect(usr.BaseUrl, usr.Name,
				usr.Password, URL.repo_data_source))) return null;
        }
        //_client.SaveSession();

        if (!(await _repo.RefreshData(appDir.Nid))) return null;

		foreach (var remF in _repo.App(appDir.Nid).Files)
			if (!files.Has(x => x.Name == remF.Name))
				files.Add(new AppFile(remF, VsRemote.NotInLocal));


		foreach (var file in files) ForwardLogs(file);
		return files;
	}



	public async Task CompareAgainstRemote(List<AppFile> localFiles, AppDir app)
	{
		var remoteFiles = _repo.App(app.Nid).Files;

		foreach (var locF in localFiles)
			await locF.CompareWith(remoteFiles,
							_fs.Folder(app.Path));
	}



	public async Task UploadChangedFiles(AppDir app, List<AppFile> list)
	{
		//Info_h("Uploading changes in files...", "Processing {0}.", list.Count.x("file"));
		var remoteFiles = _repo.App(app.Nid).Files;
		var dir = _fs.Folder(app.Path);

		foreach (var file in list)
			if (!await file.UploadTo(remoteFiles, dir)) return;
	}



	public async Task ReplaceLocals(AppDir app)
	{
		var dir = _fs.Folder(app.Path);
		foreach (var file in _repo.App(app.Nid).Files)
			await file.DownloadTo(dir);
	}



	public void Dispose()
	{
		if (_repo != null) _repo.Dispose();
	}
}
}
