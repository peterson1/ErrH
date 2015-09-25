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
        private IFileSystemShim          _fs;
        private ID7Client                _client;
        private string                   _serverDir;
        private SyncableFolderDto        _foldrNode;
        //private IRepository<AppFileNode> _remotes;


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

            return await SaveFolderNode();
        }



        private void AddToFolderNode(int fileNodeID)
            => _foldrNode?.field_files_ref.und.Add(und.TargetId(fileNodeID));

        private void RemoveFromFolderNode(int fileNodeID)
            => _foldrNode?.field_files_ref.und.Remove(und.TargetId(fileNodeID));




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
                    return await ReplaceRemoteNode(item);

                case FileTask.Delete:
                    return await DeleteRemoteNode(item);

                default:
                    return Warn_n($"Unsupported Remote NextStep: ‹{item.NextStep}›", "");
            }
        }


        private async Task<bool> SaveFolderNode()
        {
            Debug_n("Updating App node to include/exclude file nodes...", "");

            _foldrNode = await _client.Put(_foldrNode);
            if (_foldrNode.IsValidNode())
                return Debug_n("Successfully updated App node.", "");
            else
                return Warn_n("Something went wrong in updating App node.", "");
        }


        private async Task<bool> ReplaceRemoteNode(RemoteVsLocalFile inf)
        {
            var newFid  = await UploadLocalFile(inf);
            if (newFid <= 0) return false;

            //inf.Status = "Getting the freshest version of the File node...";
            //var node   = await _client.Node<SyncableFileDto>(inf.Remote.Nid);
            //if (!node.IsValidNode()) return false;

            inf.Status = "Updating node to refer to newly upload file...";
            var dto = new SyncableFileDtoRevision(inf, newFid);
            if ((await _client.Put(dto)).IsValidNode())
            {
                inf.Status = "File uploaded; node updated.";
                return Debug_n("Successfully updated File node.", "");
            }
            else
                return Warn_n("Something went wrong in updating File node.", "");
        }


        private async Task<bool> DeleteRemoteNode(RemoteVsLocalFile inf)
        {
            RemoveFromFolderNode(inf.Remote.Nid);
            if (!await SaveFolderNode()) return false;

            inf.Status = "Deleting remote file node...";
            if (!await _client.Delete(inf.Remote.Nid)) return false;

            // no need to delete the actual file by fid
            //  - Drupal 7 auto-deletes it when losing the reference to a node

            inf.Status = "Deleted remote file and node.";
            return true;
        }


        private async Task<bool> CreateRemoteNode(RemoteVsLocalFile inf)
        {
            var newFid   = await UploadLocalFile(inf);
            if (newFid  <= 0) return false;

            inf.Status   = "Creating new node...";
            var nodeDto  = new SyncableFileDto(inf.Local, newFid);
            var newNode  = await _client.Post(nodeDto);
            if (!newNode.IsValidNode()) return false;

            AddToFolderNode(newNode.nid);

            inf.Status = "File uploaded; node created.";
            return true;
        }


        private async Task<int> UploadLocalFile(RemoteVsLocalFile inf)
        {
            inf.Status  = "Uploading local file...";
            var locFile = _fs.File(inf.Local.Path);
            return await _client.Post(locFile, _serverDir);
        }
    }
}
