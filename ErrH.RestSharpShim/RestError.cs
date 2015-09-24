using System;
using System.Net;
using System.Net.Http;
using ErrH.Tools.Extensions;
using ErrH.Tools.RestServiceShim.RestExceptions;

namespace ErrH.RestSharpShim
{
    internal class RestError
    {
        public static RestServiceException Parse(HttpRequestException ex, RestSharpClientShim client, RequestShim req)
        {

            if (ex.Message.Contains("Missing required argument "))
                return RestServiceException.BadRequest("Missing required argument :  " +
                    ex.Message.Between("Missing required argument ", ").", true).Quotify(),
                        req.Method, client.BaseUrl, req.Resource, ex);

            if (ex.Message.Contains("401"))
                return RestServiceException.Unauthorized(
                    req.Method, client.BaseUrl, req.Resource, ex);

            if (ex.Message.Contains("403"))
                return RestServiceException.Forbidden(
                    req.Method, client.BaseUrl, req.Resource, ex);

            if (ex.Message.Contains("404"))
                return RestServiceException.NotFound(
                    req.Method, client.BaseUrl, req.Resource, ex);

            if (ex.Message.Contains("406"))
                return RestServiceException.NotAcceptable(
                    req.Method, client.BaseUrl, req.Resource, ex);

            if (ex.Message.Contains("Integrity constraint violation: "))
                return RestServiceException.Conflict(ex.Message
                    .Between("constraint violation: ", ").", true) + ".",
                        req.Method, client.BaseUrl, req.Resource, ex);

            if (ex.Message.Contains("Unknown data property"))
                return RestServiceException.BadRequest("Unknown data property " +
                    ex.Message.Between("Unknown data property ", ".).", true).Quotify(),
                        req.Method, client.BaseUrl, req.Resource, ex);

            if (ex.Message.Contains("value list does not match column list: "))
                return RestServiceException.BadRequest(ex.Message
                    .Between("value list does not match column list: ", ").", true) + ".",
                        req.Method, client.BaseUrl, req.Resource, ex);

            if (ex.Message.Contains("Internal Server Error : An error occurred"))
                return RestServiceException.InternalServer(ex.Message
                    .Between("An error occurred ", ").", true) + ".",
                        req.Method, client.BaseUrl, req.Resource, ex);

            if (ex.Message.Contains("Could not create destination directory"))
                return RestServiceException.InternalServer(
                    "Could not create destination directory for file.",
                        req.Method, client.BaseUrl, req.Resource, ex);


            var inr = ex.InnerException;

            if (inr == null)
                return Unknown("No inner exception", ex, client, req);

            if (inr is WebException)
                return CastWebException((WebException)inr, client, req);

            return Unknown("None of the Parse-ables", ex, client, req);
        }


        private static RestServiceException CastWebException(WebException ex, RestSharpClientShim client, RequestShim req)
        {
            var inr = ex.InnerException;

            if (inr == null)
                return Unknown("A WebException with NO innards", ex, client, req);

            if (inr.GetType().Name == "AuthenticationException")
                return RestServiceException.InvalidSsl(
                    req.Method, client.BaseUrl, req.Resource, ex);

            if (inr.GetType().Name == "SocketException")
                return RestServiceException.Unavailable(
                    req.Method, client.BaseUrl, req.Resource, ex);

            return Unknown("A WebException with UNKNOWN innards", ex, client, req);
        }



        private static RestServiceException Unknown(string clue, Exception ex, RestSharpClientShim client, RequestShim req)
        {
            return RestServiceException.Unknown(clue,
                req.Method, client.BaseUrl, req.Resource, ex);
        }
    }
}
