using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ErrH.Drupal7Client;
using ErrH.Tools.Drupal7Models;
using ErrH.Tools.Extensions;
using ErrH.Tools.FileSystemShims;
using ErrH.Tools.Loggers;
using ErrH.Uploader.Core.DTOs;
using ErrH.Uploader.Core.Nodes;

namespace ErrH.Uploader.DataAccess.AppFileRepository
{
/*
    //todo: use SlowListRepo for this or D7NodesRepoBase
    public class AppFileRepo : LogSourceBase, ID7Repo, ILogSource
    {
        internal AppFileRepo_Writer Writer { get; private set; }
        internal AppFileRepo_Downloader Downloader { get; private set; }

        private D7RepoBase<AppFileRepoDto> _repoBase;


        public bool RefreshAllApps { get; set; }

        public AppFileRepo(ID7Client d7Client, IFileSystemShim fsShim)
        {
            this._repoBase = ForwardLogs(new D7RepoBase<AppFileRepoDto>(d7Client));
            this.Writer = ForwardLogs(new AppFileRepo_Writer(d7Client, this));
            this.Downloader = ForwardLogs(new AppFileRepo_Downloader(d7Client, fsShim));
        }



        public ReadOnlyCollection<AppFolderNode> Apps
        {
            get
            {
                return _repoBase.Data.GroupBy(x => x.app_nid)
                                     .Select(x => x.First())
                                     .Select(x => AppFromDto(x))
                                     .AsReadOnly(this);
            }
        }



        public AppFolderNode App(int appNid)
        {
            var recs = FilterByAppNid(appNid);
            if (recs.Count() == 0) return null;
            return AppFromDto(recs.First());
        }



        public ReadOnlyCollection<AppFileNode> AppFiles(int appNid)
        {
            var app = this.App(appNid);
            var recs = FilterByAppNid(appNid)
                        .Where(x => x.app_file_nid.HasValue);

            if (recs.Count() == 0)
                return (new List<AppFileNode>()).AsReadOnly(app);
            else
                return recs.Select(x => AppFileNode.FromDto(x, app)).AsReadOnly(app);
        }



        private AppFolderNode AppFromDto(AppFileRepoDto dto)
        {
            var app = new AppFolderNode
            {
                Nid = dto.app_nid,
                Vid = dto.app_vid,
                Title = dto.app_title,
                //Users = dto.app_users.Split(',').Select(x => x.ToInt()).ToList(),
                Users = new List<int>(),
                Repo = this
            };

            if (!dto.app_users.IsBlank())
            {
                if (dto.app_users.Contains(","))
                    foreach (var id in dto.app_users.Split(','))
                        app.Users.Add(id.ToInt());
                else
                    app.Users.Add(dto.app_users.ToInt());
            }

            return app;
        }

        public async Task<bool> RefreshData(params object[] queryParams)
        {
            if (this.RefreshAllApps) queryParams = null;

            if (!(await _repoBase.RefreshData(queryParams))) return false;
            //if (!(await _repoBase.RefreshData())) return false;

            int n; int v; foreach (var d in _repoBase.Data)
            {
                if (!ToNidVid(d.app_nid_vid, out n, out v)) return false;
                d.app_nid = n; d.app_vid = v;

                if (!d.app_file_nid_vid.IsBlank())
                {
                    if (!ToNidVid(d.app_file_nid_vid, out n, out v)) return false;
                    d.app_file_nid = n; d.app_file_vid = v;
                }
            }
            return true;
            //if (queryParams.Length == 0)
            //	return Debug_n("Successfully refreshed repository data.", Data.Count.x("record") + " returned.");
            //else
            //	return Debug_n(Data.Count.x("record") + " returned.",
            //		Data.Select(x => "“" + x.app_file_name + "”").ToArray().Join(", "));
        }


        private bool ToNidVid(string str, out int nid, out int vid)
        {
            nid = -1; vid = -1;
            if (!str.Contains(".")) goto invalid;

            var ss = str.Split('.');
            if (ss.Length != 2 || !ss[0].IsNumeric()
                               || !ss[1].IsNumeric())
                goto invalid;

            nid = ss[0].ToInt();
            vid = ss[1].ToInt();
            return true;

            invalid: return Error_n("Unexpected nid_vid format:", str);
        }






        private List<AppFileRepoDto> FilterByAppNid(int appNid)
        {
            var recs = _repoBase.Data.Where(x => x.app_nid == appNid).ToList();
            if (recs.Count() == 0)
                Warn_n("Cannot find requested App node.", "where App nid = " + appNid);
            return recs;
        }


        internal AppFileNode AppFileByNid(int appFileNid)
        {
            var recs = _repoBase.Data.Where(x => x.app_file_nid == appFileNid);
            if (recs.Count() == 0)
            {
                Warn_n("Cannot find requested App-File node.", "where App-File nid = " + appFileNid);
                return null;
            }
            var app = this.App(recs.Single().app_nid);

            return AppFileNode.FromDto(recs.Single(), app);
        }



        public async Task<bool> Connect(string baseUrl, string username, string password, string resource)
        {
            if (!(await _repoBase.Connect(baseUrl, username, password, resource))) return false;
            if (RefreshAllApps) return await RefreshData();
            return true;
        }

        public async Task<bool> Disconnect() { return await _repoBase.Disconnect(); }

        public bool IsConnected { get { return _repoBase.IsConnected; } }

        public void Dispose() { _repoBase.Dispose(); }

    }
*/
}
