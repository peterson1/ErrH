using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ErrH.Tools.Drupal7Models.Entities;
using ErrH.Tools.Loggers;
using ErrH.Tools.ScalarEventArgs;

namespace ErrH.Tools.RestServiceShim
{
    public interface IClientShim : ILogSource
    {
        event EventHandler<EArg<bool>> ResponseReceived;

        int RetryIntervalSeconds { get; set; }

        string BaseUrl { get; set; }

        Task<T> Send<T>(IRequestShim req,
                        CancellationToken cancelToken,
                        string taskIntro = null,
                        string successMessage = null,
                        params Func<T, object>[] successMsgArgs
                        );

        Task<IResponseShim> Send(IRequestShim req,
                                 CancellationToken cancelToken,
                                 string taskIntro = null,
                                 object successMessage = null,
                                 params object[] successMsgArgs);


        Task<bool> Send<T>( IEnumerable<IRequestShim> list
                          , int pageSize = 10
                          , CancellationToken tkn = new CancellationToken()
                          ) where T : ID7Node, new();

        //D7User CurrentUser { get; set; }
    }
}
