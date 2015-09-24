using System.Collections.Generic;
using System.Threading.Tasks;
using ErrH.Tools.Drupal7Models;
using ErrH.Tools.Drupal7Models.Entities;
using ErrH.Tools.Drupal7Models.Fields;
using ErrH.Tools.Extensions;
using ErrH.Tools.FileSynchronization;
using ErrH.Tools.FileSystemShims;
using ErrH.Tools.Loggers;

namespace ErrH.Drupal7FileUpdater
{
    public class D7FileSynchronizer : LogSourceBase, IFileSynchronizer
    {
        private IFileSystemShim   _fs;
        private ID7Client         _client;
        private string            _serverDir;
        private SyncableFolderDto _foldrNode;


        public D7FileSynchronizer(ID7Client d7Client, IFileSystemShim fsShim)
        {
            _client = d7Client;
            _fs = fsShim;
        }


        public async Task<bool> Run(int folderNid, List<RemoteVsLocalFile> list, string serverDir)
        {
            _serverDir = serverDir;
            _foldrNode = await _client.Node<SyncableFolderDto>(folderNid);
            if (_foldrNode == null)
                return Warn_h("= = = = = =   How to Fix   = = = = = =",
                    L.F + "  - This may happen if the list of files is empty." 
                  + L.f + "  - You may want to add a temporary file using the web UI, which you can delete later.");

            foreach (var item in list)
            {
                switch (item.Target)
                {
                    case Target.Remote:
                        await ActOnRemote(item);
                        break;

                    case Target.Local:
                        await ActOnLocal(item);
                        break;

                    case Target.Both:
                        await ActOnRemote(item);
                        await ActOnLocal(item);
                        break;

                    default:
                        Warn_n($"Unsupported Target: ‹{item.Target}›", "");
                        break;
                }
            }

            Debug_n("Updating App node to include new file nodes...", "");
            var ret = await _client.Put(_foldrNode);
            if (ret.IsValidNode())
                Debug_n("Successfully updated App node.", "");
            else
                Warn_n("Something went wrong in updating App node.","");

            return true;
        }


        private void AddToFolderNode(int fileNodeID)
            => _foldrNode?.field_files_ref.und.Add(und.TargetId(fileNodeID));




        private async Task<bool> ActOnLocal(RemoteVsLocalFile item)
        {
            switch (item.NextStep)
            {
                case FileTask.Ignore:
                    return true;

                case FileTask.Analyze:
                    return Warn_n("Not yet implemented.", "Analyze in Local");

                case FileTask.Create:
                    await TaskEx.Delay(1);
                    return Warn_n("Not yet implemented.", "Create in Local");

                case FileTask.Replace:
                    return Warn_n("Not yet implemented.", "Replace in Local");

                case FileTask.Delete:
                    return Warn_n("Not yet implemented.", "Delete in Local");

                default:
                    return Warn_n($"Unsupported Local NextStep: ‹{item.NextStep}›", "");
            }
        }

        private async Task<bool> ActOnRemote(RemoteVsLocalFile item)
        {
            switch (item.NextStep)
            {
                case FileTask.Ignore:
                    return true;

                case FileTask.Analyze:
                    return Warn_n("Not yet implemented.", "Analyze in Remote");

                case FileTask.Create:
                    return await CreateRemoteNode(item);

                case FileTask.Replace:
                    return Warn_n("Not yet implemented.", "Replace in Remote");

                case FileTask.Delete:
                    return Warn_n("Not yet implemented.", "Delete in Remote");

                default:
                    return Warn_n($"Unsupported Remote NextStep: ‹{item.NextStep}›", "");
            }
        }


        private async Task<bool> CreateRemoteNode(RemoteVsLocalFile item)
        {
            var src      = item.Local;
            var locFile  = _fs.File(src.UrlOrPath);

            item.Status  = "Uploading file...";
            var newFid   = await _client.Post(locFile, _serverDir);
            if (newFid <= 0) return false;

            item.Status  = "Adding new node...";
            var nodeDto  = new SyncableFileDto(src, newFid);
            var newNode  = await _client.Post(nodeDto);
            if (!newNode.IsValidNode()) return false;

            AddToFolderNode(newNode.nid);

            item.Status = "New node Added.";

            return true;
        }
    }
}
