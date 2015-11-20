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

        ISqlDbReader  DbReader    { get; set; }
        string        ResourceURL { get; }
        string        SqlQuery    { get; }


        Task<bool>  Update  (
            IRepository<T> repository,
            IMapOverride rowMapperOverride = null,
            CancellationToken token = new CancellationToken()
        );
    }
}
