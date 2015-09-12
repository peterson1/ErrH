using System;
using System.Net;

namespace ErrH.Tools.RestServiceShim.RestExceptions
{
    public class InvalidSslRestException : RestServiceException
    {
        public InvalidSslRestException(string msg, HttpStatusCode code, RestMethod method, string baseUrl, string resource, Exception inr = null) 
            : base(msg, code, method, baseUrl, resource, inr)
        {
        }
    }
}
