using System.Collections.Generic;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.Extensions;
using ErrH.Tools.FileSynchronization;
using ErrH.Tools.Loggers;

namespace ErrH.Uploader.Core.Services
{
    public class AppFileGrouper : LogSourceBase
    {
        private LocalFileSeeker _fileSeeker;


        public AppFileGrouper(LocalFileSeeker fileSeeker)
        {
            _fileSeeker = ForwardLogs(fileSeeker);
        }


        /// <summary>
        /// This method assumes that RemoteRepo.Load() was already called.
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public List<RemoteVsLocalFile> GroupFilesByName
            (SyncableFolderInfo app, IRepository<SyncableFileRemote> repo)
        {
            var list   = new List<RemoteVsLocalFile>();
            var locals = _fileSeeker.GetFiles(app.Path);


            foreach (var loc in locals)
            {
                var rem = repo.One(x => x.Name == loc.Name);
                list.Add(RemVsLoc(loc, rem));
            }


            foreach (var rem in repo.Any(r
                => !list.Has(l => l.Filename == r.Name)))
            {
                var loc = locals.One(x => x.Name == rem.Name);
                list.Add(RemVsLoc(loc, rem));
            }


            //  if no files in folder, or no folder at all,
            //    - assume that it's a first run : download all
            //
            if (locals.Count == 0)
                list.ForEach(x => x.DoNext(Target.Local, FileTask.Create));


            return list;
        }


        private RemoteVsLocalFile RemVsLoc(SyncableFileLocal locFile, SyncableFileRemote remNode)
        {
            //return new RemoteVsLocalFile(locFile?.Name ?? remNode.Name)
            //{
            //    Local = locFile,
            //    Remote = RemoteFileInfo(remNode)
            //};
            var fName = locFile?.Name ?? remNode.Name;
            var remFile = RemoteFileInfo(remNode);
            return new RemoteVsLocalFile(fName, remFile, locFile);
        }



        private SyncableFileRemote RemoteFileInfo(SyncableFileRemote rem)
        {
            if (rem == null) return null;
            return new SyncableFileRemote
            {
                Name      = rem.Name,
                Size      = rem.Size,
                Version   = rem.Version ?? "",
                SHA1      = rem.SHA1,
                Nid       = rem.Nid,
                Vid       = rem.Vid,
                Fid       = rem.Fid
                //UrlOrPath = $"fid: {rem.Fid}"
            };
        }
    }
}
