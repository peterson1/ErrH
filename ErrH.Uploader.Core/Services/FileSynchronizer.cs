using System;
using System.Collections.Generic;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.Drupal7Models;
using ErrH.Tools.ErrorConstructors;
using ErrH.Tools.FileSystemShims;
using ErrH.Tools.Loggers;
using ErrH.Uploader.Core.Models;
using ErrH.Uploader.Core.Nodes;

namespace ErrH.Uploader.Core.Services
{
    public class FileSynchronizer : LogSourceBase
    {
        private IRepository<AppFileNode> _remote;
        private ID7Client _client;
        private IFileSystemShim _fs;



        public FileSynchronizer(IRepository<AppFileNode> appFileRepo, ID7Client d7Client, IFileSystemShim fsShim)
        {
            _remote = ForwardLogs(appFileRepo);
            _client = d7Client;
            _fs     = fsShim;
        }


        public bool Run(List<RemoteVsLocalFile> list)
        {
            foreach (var item in list)
            {
                switch (item.Target)
                {
                    case Target.Remote:
                        if (!ActOnRemote(item)) return false;
                        break;

                    case Target.Local:
                        if (!ActOnLocal(item)) return false;
                        break;

                    case Target.Both:
                        if (!ActOnRemote(item) 
                         || !ActOnLocal(item)) return false;
                        break;
                    default:
                        throw Error.Unsupported(item.Target);
                }
            }
            return true;
        }

        private bool ActOnLocal(RemoteVsLocalFile item)
        {
            throw new NotImplementedException();
        }

        private bool ActOnRemote(RemoteVsLocalFile item)
        {
            switch (item.NextStep)
            {
                case Action.Ignore:
                    break;
                //case Action.Analyze:
                //    break;
                case Action.Create:
                    return CreateRemoteNode(item);
                //case Action.Replace:
                //    break;
                //case Action.Delete:
                //    break;
                default:
                    throw Error.Unsupported(item.NextStep);
            }
            return false;
        }


        private bool CreateRemoteNode(RemoteVsLocalFile item)
        {
            var src = item.Local;
            var locFile = _fs.File(src.UrlOrPath);

            item.Status = "Uploading file...";
            //var newFid = _client.Post(locFile, SERVER_DIR.app_files).Result;
            var newFid = -1;

            item.Status = "Adding new node...";
            var node = new AppFileNode
            {
                Name    = item.Filename,
                SHA1    = src.SHA1,
                Size    = src.Size,
                Version = src.Version,
                Fid     = newFid
            };
            _remote.Add(node);

            item.Status = "New node Added.";
            return true;
        }
    }
}
