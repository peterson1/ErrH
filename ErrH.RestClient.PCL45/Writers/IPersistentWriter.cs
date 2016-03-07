using System;
using System.Threading;
using System.Threading.Tasks;
using ErrH.RestClient.PCL45.Drupal7Models;
using ErrH.RestClient.PCL45.EventArguments;

namespace ErrH.RestClient.PCL45.Writers
{
    public interface IPersistentWriter
    {
        event EventHandler<EArg<string>> AttemptFailed;

        Task<ID7PutDTO> Post<T>(T d7Node,
            CancellationToken cancelr, int delaySeconds = 4)
            where T : ID7PostDTO;
    }
}
