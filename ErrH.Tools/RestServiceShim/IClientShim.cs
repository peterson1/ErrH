using System;
using System.Threading.Tasks;
using ErrH.Tools.Loggers;

namespace ErrH.Tools.RestServiceShim
{
    public interface IClientShim : ILogSource
    {
        string BaseUrl { get; set; }

        Task<T> Send<T>(IRequestShim req,
                        string taskIntro = null,
                        string successMessage = null,
                        params Func<T, object>[] successMsgArgs
                        ) where T : new();

        Task<IResponseShim> Send(IRequestShim req,
                                 string taskIntro = null,
                                 object successMessage = null,
                                 params object[] successMsgArgs);

        //D7User CurrentUser { get; set; }
    }
}
