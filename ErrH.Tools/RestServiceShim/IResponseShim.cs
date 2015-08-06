using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ErrH.Tools.RestServiceShim
{
    public interface IResponseShim
    {
        IRequestShim Request { get; }
        HttpStatusCode Code { get; }
        string Message { get; }
        string BaseUrl { get; }
        string Content { get; }
        bool IsSuccess { get; }
        Exception Error { get; }
    }
}
