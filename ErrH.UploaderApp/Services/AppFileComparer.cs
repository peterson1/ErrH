using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ErrH.Tools.Extensions;
using ErrH.Tools.FileSystemShims;
using ErrH.UploaderApp.AppFileRepository;
using ErrH.UploaderApp.Models;

namespace ErrH.UploaderApp.Services
{
    internal static class AppFileComparer
    {
        internal static async Task CompareWith(this AppFile appF, 
                                               ReadOnlyCollection<AppFileItem> remoteFiles, 
                                               FolderShim parentDir)
        {
            appF.Compared = VsRemote.Checking;
            appF.location = "remote :" + L.f + "local :";

            var remoteF = appF.FindIn(remoteFiles);
            var localF = appF.FindIn(parentDir);
            if (localF == null || remoteF == null) return;


            if (localF.Size != remoteF.Size)
            {
                appF.Compared = VsRemote.Changed;
                appF.Difference = "different sizes";
                return;
            }

            appF.Compared = await appF.CompareHashes(localF, remoteF);
        }


        private static async Task<VsRemote> CompareHashes(this AppFile appF,
                                                          FileShim locF, 
                                                          AppFileItem remF)
        {
            var locSHA1 = await TaskEx.Run(() => { return locF.SHA1; });

            var state = (locSHA1 == remF.SHA1) ? VsRemote.Same
                                               : VsRemote.Changed;

            appF.Difference = (state == VsRemote.Same)
                            ? "‹no diff›" : "different hashes";
            return state;
        }



        private static FileShim FindIn(this AppFile appF,
                                       FolderShim parentDir)
        {
            appF.Sizes += L.f;
            appF.Versions += L.f;

            var localF = parentDir.File(appF.Name, false);
            if (!localF.Found)
            {
                appF.Compared = VsRemote.NotInLocal;
                appF.Difference = "not in local";
                return null;
            }
            appF.Sizes += localF.Size.KB();
            appF.Versions += " v." + localF.Version;
            return localF;
        }





        private static AppFileItem FindIn(this AppFile appF,
                                          ReadOnlyCollection<AppFileItem> remoteFiles)
        {
            var matches = remoteFiles.Where(x => x.Name == appF.Name);
            if (matches.Count() == 0)
            {
                appF.Compared = VsRemote.NotInRemote;
                appF.Difference = "not in remote";
                return null;
            }
            if (matches.Count() > 1)
            {
                appF.Compared = VsRemote.MultipleInRemote;
                return null;
            }
            var remoteF = matches.Single();
            appF.Sizes = remoteF.Size.KB();
            appF.Versions = " v." + remoteF.Version;
            return remoteF;
        }

    }
}
