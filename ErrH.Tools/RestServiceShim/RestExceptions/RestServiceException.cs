using System;
using System.Net;
using ErrH.Tools.Extensions;

namespace ErrH.Tools.RestServiceShim.RestExceptions
{
    public class RestServiceException : WebException
    {
        public readonly HttpStatusCode Code;
        public readonly RestMethod Method;
        public readonly string BaseUrl;
        public readonly string Resource;

        public RestServiceException(string msg,
                                    HttpStatusCode code,
                                    RestMethod method,
                                    string baseUrl,
                                    string resource,
                                    Exception inr = null)
        : base(msg, inr)
        {
            this.Code = code;
            this.Method = method;
            this.BaseUrl = baseUrl;
            this.Resource = resource;
        }


        public static RestServiceException Unknown(string clue,
            RestMethod method, string baseUrl, string resource, Exception inr)
        {
            var msg = "Unknown REST Service exception."
                    + L.f + "clue :" + clue
                    + L.f + "type :" + inr.GetType().Name;

            return new RestServiceException(msg,
                HttpStatusCode.InternalServerError,
                method, baseUrl, resource, inr);
        }


        public static InvalidSslRestException InvalidSsl(
            RestMethod method, string baseUrl, string resource, Exception ex)
        {
            return new InvalidSslRestException("SSL certificate is invalid.",
                HttpStatusCode.Forbidden,
                method, baseUrl, resource, ex);
        }


        public static RestServiceException Unavailable(
            RestMethod method, string baseUrl, string resource, Exception ex)
        {
            return new RestServiceException("Server is currently unavailable.",
                HttpStatusCode.ServiceUnavailable,
                method, baseUrl, resource, ex);
        }


        public static RestServiceException Unauthorized(
            RestMethod method, string baseUrl, string resource, Exception ex)
        {
            return new RestServiceException("Attempted to authenticate, but failed.",
                HttpStatusCode.Unauthorized,
                method, baseUrl, resource, ex);
        }


        public static RestServiceException Forbidden(
            RestMethod method, string baseUrl, string resource, Exception ex)
        {
            return new RestServiceException("Server refused to fulfill the request.",
                HttpStatusCode.Forbidden,
                method, baseUrl, resource, ex);
        }


        public static RestServiceException NotFound(
            RestMethod method, string baseUrl, string resource, Exception ex)
        {
            return new RestServiceException("Requested resource does not exist on the server.",
                HttpStatusCode.NotFound,
                method, baseUrl, resource, ex);
        }


        public static RestServiceException NotAcceptable(
            RestMethod method, string baseUrl, string resource, Exception ex)
        {
            var msg = ex.Message.Between("406 (Not Acceptable : ", ").", true);
            return new RestServiceException(msg,
                HttpStatusCode.NotAcceptable,
                method, baseUrl, resource, ex);
        }


        public static RestServiceException BadRequest(
            string specificMsg, RestMethod method, string baseUrl, string resource, Exception ex)
        {
            return new RestServiceException(specificMsg,
                HttpStatusCode.BadRequest,
                method, baseUrl, resource, ex);
        }


        public static RestServiceException Conflict(
            string conflictMsg, RestMethod method, string baseUrl, string resource, Exception ex)
        {
            return new RestServiceException(conflictMsg,
                HttpStatusCode.Conflict,
                method, baseUrl, resource, ex);
        }


        public static RestServiceException InternalServer(
            string shortMsg, RestMethod method, string baseUrl, string resource, Exception ex)
        {
            return new RestServiceException(shortMsg,
                HttpStatusCode.InternalServerError,
                method, baseUrl, resource, ex);
        }

    }
}
