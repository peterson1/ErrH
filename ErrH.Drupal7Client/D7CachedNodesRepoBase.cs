using System.Threading;
using System.Threading.Tasks;
using ErrH.Tools.Authentication;
using ErrH.Tools.FileSystemShims;

namespace ErrH.Drupal7Client
{
    public abstract class D7CachedNodesRepoBase<TNodeDto, TClass> : D7NodesRepoBase<TNodeDto, TClass>
    {
        private IFileSystemShim _fs;


        protected abstract string SubURL { get; }


        public D7CachedNodesRepoBase(IFileSystemShim fileSystemShim, ISessionClient client, IBasicAuthenticationKey credentials)
        {
            _fs = ForwardLogs(fileSystemShim);
            SetClient(ForwardLogs(client), credentials);
        }

        public D7CachedNodesRepoBase(IFileSystemShim fileSystemShim, IClientSource clientSource)
        {
            _fs = ForwardLogs(fileSystemShim);
            SetClient(ForwardLogs(clientSource.Client), clientSource.AuthKey);
        }


        public override async Task<bool> LoadAsync(CancellationToken tkn, params object[] args)
        {
            //todo: find local cache first
            return await base.LoadAsync(tkn, SubURL);
        }
    }
}
