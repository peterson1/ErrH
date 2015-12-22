using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ErrH.Drupal7FileUpdater.Services;
using ErrH.Tools.Drupal7Models;
using ErrH.Tools.Drupal7Models.Entities;
using ErrH.Tools.Drupal7Models.Fields;
using ErrH.Tools.FileSynchronization;
using ErrH.Tools.FileSystemShims;
using ErrH.Tools.Loggers;

namespace ErrH.Drupal7FileUpdater
{
    public class D7FileSynchronizer : LogSourceBase, IFileSynchronizer
    {
        private IFileSystemShim   _fs;
        private ID7Client         _client;
        private string            _targetDir;
        private int               _foldrNid;
        private SyncableFolderDto _foldrNode;
        private CancellationToken _cancelToken;
        private D7FileDownloader  _downloadr;


        public bool HasReplacement { get; private set; }


        public D7FileSynchronizer(IFileSystemShim fsShim)
        {
            _fs = ForwardLogs(fsShim);
        }


        public async Task<bool> Run(int folderNid, List<RemoteVsLocalFile> list, string targetDir, CancellationToken cancelToken, string subUrlPattern)
        {
            HasReplacement = false;
            _targetDir     = targetDir;
            _foldrNid      = folderNid;
            _cancelToken   = cancelToken;
            _downloadr     = ForwardLogs(new D7FileDownloader(_targetDir, 
                                            subUrlPattern, _fs, _client));
            _foldrNode = null;

            foreach (var item in list)
            {
                switch (item.Target)
                {
                    case Target.Remote:
                        await ActOnRemote(item, cancelToken);
                        break;

                    case Target.Local:
                        await ActOnLocal(item, cancelToken);
                        break;

                    case Target.Both:
                        await ActOnRemote(item, cancelToken);
                        await ActOnLocal(item, cancelToken);
                        break;

                    default:
                        Warn_n($"Unsupported Target: ‹{item.Target}›", "");
                        break;
                }
            }

            if (_foldrNode != null)
                return await SaveFolderNode(cancelToken);
            else
                return true;
        }


        public void SetClient(ID7Client d7Client)
            => _client = ForwardLogs(d7Client);



        private async Task<bool> AddToFolderNode(int fileNodeID)
        {
            if (!await LoadFolderNodeIfNull()) return false;
            _foldrNode?.field_files_ref.und.Add(und.TargetId(fileNodeID));
            return true;
        }


        private async Task<bool> RemoveFromFolderNode(int fileNodeID)
        {
            if (!await LoadFolderNodeIfNull()) return false;
            _foldrNode.field_files_ref.und.Remove(und.TargetId(fileNodeID));
            return true;
        }


        private async Task<bool> LoadFolderNodeIfNull()
        {
            if (_foldrNode != null) return true;

            _foldrNode = await _client.Node<SyncableFolderDto>(_foldrNid, _cancelToken);

            // set the flag to disable downloads
            _foldrNode.field_currently_updating.und.Clear();
            _foldrNode.field_currently_updating.und.Add(und.Value(1));

            _foldrNode = await _client.Put(_foldrNode, _cancelToken);
            if (_foldrNode.IsValidNode())
                return Debug_n("Flag set to temporarily disable downloads.", "");
            else
                return Warn_n("Something went wrong in updating App node.", "");
        }




        private async Task<bool> ActOnLocal(RemoteVsLocalFile item, CancellationToken cancelToken)
        {
            switch (item.NextStep)
            {
                case FileTask.Ignore:
                    return true;

                case FileTask.Analyze:
                    return Warn_n("Not yet implemented.", "Analyze in Local");

                case FileTask.Create:
                    return await _downloadr.CreateFile(item, cancelToken);

                case FileTask.Replace:
                    HasReplacement = true;
                    return await _downloadr.ReplaceFile(item, cancelToken);

                case FileTask.Delete:
                    return Warn_h("Task should not be allowed.", "Delete in Local");

                default:
                    return Warn_n($"Unsupported Local NextStep: ‹{item.NextStep}›", "");
            }
        }



        private async Task<bool> ActOnRemote(RemoteVsLocalFile item, CancellationToken cancelToken)
        {
            switch (item.NextStep)
            {
                case FileTask.Ignore:
                    return true;

                case FileTask.Analyze:
                    return Warn_n("Not yet implemented.", "Analyze in Remote");

                case FileTask.Create:
                    return await CreateRemoteNode(item, cancelToken);

                case FileTask.Replace:
                    return await ReplaceRemoteNode(item, cancelToken);

                case FileTask.Delete:
                    return await DeleteRemoteNode(item, cancelToken);

                default:
                    return Warn_n($"Unsupported Remote NextStep: ‹{item.NextStep}›", "");
            }
        }


        private async Task<bool> SaveFolderNode(CancellationToken cancelToken)
        {
            Debug_n("Updating App node to include/exclude file nodes...", "");


            // unset the flag to enable downloads
            _foldrNode.field_currently_updating.und.Clear();
            _foldrNode.field_currently_updating.und.Add(und.Value(0));

            _foldrNode = await _client.Put(_foldrNode, cancelToken);
            if (_foldrNode.IsValidNode())
                return Debug_n("Successfully updated App node.", "");
            else
                return Warn_n("Something went wrong in updating App node.", "");
        }


        private async Task<bool> ReplaceRemoteNode(RemoteVsLocalFile inf, CancellationToken cancelToken)
        {
            if (!await LoadFolderNodeIfNull()) return false;

            var newFid  = await UploadLocalFile(inf, cancelToken);
            if (newFid <= 0) return false;

            //inf.Status = "Getting the freshest version of the File node...";
            //var node   = await _client.Node<SyncableFileDto>(inf.Remote.Nid);
            //if (!node.IsValidNode()) return false;

            inf.Status = "Updating node to refer to newly upload file...";
            var dto = new SyncableFileDtoRevision(inf, newFid);
            if ((await _client.Put(dto, cancelToken)).IsValidNode())
            {
                inf.Status = "File uploaded; node updated.";
                return Debug_n("Successfully updated File node.", "");
            }
            else
                return Warn_n("Something went wrong in updating File node.", "");
        }


        private async Task<bool> DeleteRemoteNode(RemoteVsLocalFile inf, CancellationToken cancelToken)
        {
            if (!await RemoveFromFolderNode(inf.Remote.Nid)) return false;
            if (!await SaveFolderNode(cancelToken)) return false;

            inf.Status = "Deleting remote file node...";
            if (!await _client.Delete(inf.Remote.Nid, cancelToken)) return false;

            // no need to delete the actual file by fid
            //  - Drupal 7 auto-deletes it when losing the reference to a node

            inf.Status = "Deleted remote file and node.";
            return true;
        }


        private async Task<bool> CreateRemoteNode(RemoteVsLocalFile inf, CancellationToken cancelToken)
        {
            var newFid   = await UploadLocalFile(inf, cancelToken);
            if (newFid  <= 0) return false;

            inf.Status   = "Creating new node...";
            var nodeDto  = new SyncableFileDto(inf.Local, newFid);
            var newNode  = await _client.Post(nodeDto, cancelToken);
            if (!newNode.IsValidNode()) return false;

            if (await AddToFolderNode(newNode.nid)) return false;

            inf.Status = "File uploaded; node created.";
            return true;
        }


        private async Task<int> UploadLocalFile(RemoteVsLocalFile inf, CancellationToken cancelToken)
        {
            inf.Status  = "Uploading local file...";
            var locFile = _fs.File(inf.Local.Path);
            return await _client.Post(locFile, cancelToken, _targetDir);
        }

    }
}
