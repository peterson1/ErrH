using System;
using System.Threading;
using System.Threading.Tasks;
using ErrH.RestClient.PCL45.EventArguments;

namespace ErrH.RestClient.PCL45.Writers
{
    public interface IPersistentWriter
    {
        event EventHandler<EArg<string>> AttemptFailed;

        int RetryDelaySeconds { get; set; }

        Task<T> Post<T>(T d7Node, CancellationToken cancelr, 
            string resource = "/entity_node/");
    }
}
