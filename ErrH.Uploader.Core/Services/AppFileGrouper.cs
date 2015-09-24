using System.Collections.Generic;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.Extensions;
using ErrH.Tools.Loggers;
using ErrH.Uploader.Core.Models;
using ErrH.Uploader.Core.Nodes;

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
            (AppFolder app, IRepository<AppFileNode> repo)
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
                list.ForEach(x => x.DoNext(Target.Local, Action.Create));


            return list;
        }


        private RemoteVsLocalFile RemVsLoc(AppFileInfo locFile, AppFileNode remNode)
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



        private AppFileInfo RemoteFileInfo(AppFileNode rem)
        {
            if (rem == null) return null;
            return new AppFileInfo
            {
                Name      = rem.Name,
                Size      = rem.Size,
                Version   = rem.Version,
                SHA1      = rem.SHA1,
                UrlOrPath = $"fid: {rem.Fid}"
            };
        }
    }
}
