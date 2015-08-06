using System;
using System.Net;
using ErrH.Tools.FileSystemShims;
using ErrH.Tools.RestServiceShim;

namespace ErrH.Tools.ErrorConstructors
{
    public class Throw
    {
        public static void Unsupported(object obj, string description = "parameter")
        {
            throw Error.Unsupported(obj, description);
        }


        public static void BadCast<T>(object obj) where T : class
        {
            throw Error.InCast<T>(obj);
        }


        public static void NoMember(string memberName)
        {
            throw Error.NoMember(memberName);
        }


        public static void IfNull<T>(T nullableObj, string description)
        {
            if (nullableObj == null)
                throw Error.NullRef($"‹{typeof(T).Name}› {description}");
        }


        public static void Unauthorized(string message, Exception inner = null)
        {
            throw Error.Unauthorized(message, inner);
        }


        public static void TimedOut(string message)
        {
            throw Error.TimedOut(message);
        }


        public static void Undone(string methodName)
        {
            throw Error.Undone(methodName);
        }


        public static void BadData(string message)
        {
            throw Error.BadData(message);
        }


        public static void Missing(FileShim fileShim)
        {
            throw Error.MissingFile(fileShim.Path);
        }


        public static void BadAct(string invalidOperationMsg)
        {
            throw Error.BadAct(invalidOperationMsg);
        }


        public static void Rest(string msg, IResponseShim resp)
        {
            throw Error.Rest(msg, resp);
        }

        public static void Rest(string msg,
                                HttpStatusCode code,
                                RestMethod method,
                                string baseUrl,
                                string resource,
                                Exception inr = null)
        {
            throw Error.Rest(msg, code, method, baseUrl, resource, inr);
        }

    }
}
