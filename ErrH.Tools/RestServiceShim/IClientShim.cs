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


        Task<bool> Send<T>( CancellationToken tkn
                          , List<IRequestShim> list
            ) where T : D7NodeBase, new();

        //D7User CurrentUser { get; set; }
    }
}
