using System;
using System.Threading;
using System.Threading.Tasks;
using ErrH.Tools.Loggers;
using ErrH.Tools.SqlHelpers;

namespace ErrH.Tools.CollectionShims
{
    public interface IRepoUpdater<T> 
        : ILogSource, IDisposable
    {

        ISqlDbReader DbReader { get; set; }

        Task<bool>  Update  (
            IRepository<T> repository,
            string resourceURL,
            CancellationToken token = new CancellationToken()
        );
    }
}
