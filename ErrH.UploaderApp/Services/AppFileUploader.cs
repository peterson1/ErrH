//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Threading.Tasks;
//using ErrH.Tools.FileSystemShims;
//using ErrH.UploaderApp.AppFileRepository;
//using ErrH.UploaderApp.Models;

//namespace ErrH.UploaderApp.Services
//{
//    internal static class AppFileUploader
//    {

//        internal static async Task<bool> UploadTo(this AppFileDiff appF, 
//                                                  ReadOnlyCollection<AppFileNode> list, 
//                                                  FolderShim dir)
//        {
//            appF.Info_h($"Processing “{appF.Name}”...", 
//                        $"against remote = ‹{appF.Compared}›");
//            var ok = false;
//            appF.State = Sending.OnGoing;
//            var node = list.SingleOrDefault(x => x.Name == appF.Name);

//            if (appF.Compared == VsRemote.Same)
//            {
//                ok = appF.Debug_n("Local file same as remote.", 
//                                  "No need to upload.");
//                goto SetState;
//            }

//            if (appF.Compared == VsRemote.NotInLocal)
//            {
//                appF.Warn_n($"“{appF.Name}” does not exist in local.", 
//                            "To keep them in-sync, server node will be deleted.");
//                ok = await node.Delete();
//                goto SetState;
//            }

//            var fsFile = dir.File(appF.Name);
//            switch (appF.Compared)
//            {
//                case VsRemote.NotInRemote:
//                    appF.Debug_n($"“{appF.Name}” does not exist in remote.", 
//                                 "A new node will be created for this file.");
//                    ok = await list.TryAdd(fsFile);
//                    break;

//                case VsRemote.Changed:
//                    appF.Debug_n("Replacing remote file with the local copy...", 
//                                $"“{appF.Name}” : {appF.Difference}.");
//                    ok = await node.TryReplaceFile(fsFile);
//                    break;

//                default:
//                    ok = appF.Warn_n("Invalid AppFile state.", 
//                                    $"Cannot upload if Compared = {appF.Compared}.");
//                    break;
//            }

//            //if (ok) Info_n("Successfully uploaded “{0}”".f(this.Name), "");

//            SetState:
//            appF.State = ok ? Sending.Finished : Sending.Error;
//            return ok;
//        }

//    }
//}
