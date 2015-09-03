using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrH.Tools.CollectionShims;
using ErrH.Uploader.Core.Nodes;
using ErrH.Uploader.Core.Services;
using ErrH.WpfTools.ViewModels;

namespace ErrH.Uploader.ViewModels.ContentVMs
{
    public class FilesTabVM2 : ListWorkspaceVMBase<FileDiffVM2>
    {
        private IRepository<AppFileNode> _remotes;
        private AppFileGrouper _locals;


        public FilesTabVM2(IRepository<AppFileNode> filesRepo,
                           AppFileGrouper fileGrouper)
        {
            _remotes = ForwardLogs(filesRepo);
            _locals = ForwardLogs(fileGrouper);
        }


        protected override Task<List<FileDiffVM2>> CreateVMsList()
        {
            throw new NotImplementedException();
        }
    }
}
