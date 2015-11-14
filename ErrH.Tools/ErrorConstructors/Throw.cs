using System;
using System.Net;
using ErrH.Tools.Extensions;
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
            throw Error.BadCast<T>(obj);
        }


        public static void NoMember(string memberName)
        {
            throw Error.NoMember(memberName);
        }


        public static void NoMember<T>(string memberName)
        {
            throw Error.NoMember<T>(memberName);
        }


        public static void IfBlank(string argValue, string argName)
        {
            if (argValue.IsBlank())
                Throw.BadArg(argName, "should not be blank");
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


        public static void BadArg(string argName, string shouldBeMsg)
        {
            throw Error.BadArg(argName, shouldBeMsg);
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
