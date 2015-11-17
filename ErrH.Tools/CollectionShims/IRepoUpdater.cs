using System;
using System.Threading;
using System.Threading.Tasks;
using ErrH.Tools.Loggers;

namespace ErrH.Tools.CollectionShims
{
    public interface IRepoUpdater<T> 
        : ILogSource, IDisposable
    {

        Task<bool>  Update  (
            IRepository<T> repository,
            CancellationToken token = new CancellationToken()
        );

    }
}
