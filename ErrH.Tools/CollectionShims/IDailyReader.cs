using System;
using System.Threading;
using System.Threading.Tasks;
using ErrH.Tools.Loggers;
using ErrH.Tools.ScalarEventArgs;

namespace ErrH.Tools.CollectionShims
{
    public interface IDailyReader<TIn, TOut> 
        : ILogSource
        where TOut : struct
    {
        event EventHandler<EArg<DateTime>> LoadedFromServer;

        void RaiseLoadedFromServer(DateTime date);

        Task<bool> LoadTxnDay(DateTime date, 
            CancellationToken token = new CancellationToken());
    }
}
