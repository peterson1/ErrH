using System;
using System.IO;
using System.Net;
using ErrH.Tools.Extensions;
using ErrH.Tools.RestServiceShim;
using ErrH.Tools.RestServiceShim.RestExceptions;

namespace ErrH.Tools.ErrorConstructors
{
    public class Error
    {
        public static FormatException MaxLength(int charLimit, string propertyName, string invalidValue)
        {
            var msg1 = "[{0}] should not be longer than {1} characters."
                        .f(propertyName, charLimit);

            var msg2 = "The text \"{0}\" is {1} characters long."
                        .f(invalidValue, invalidValue.Length);

            return new FormatException(".\n" + msg1 + "\n" + msg2);
        }


        public static InvalidCastException BadCast<T>(object obj) where T : class
        {
            var fmt = "." + L.f
                    + "Unable to cast from {0} to type {1}." + L.f
                    + "value : \t {2}";
            var msg = fmt.f(
                obj.GetType().Name.Guillemet(),
                typeof(T).Name.Guillemet(),
                obj.ToString()
            );

            return new InvalidCastException(msg);
        }


        public static NotSupportedException Unsupported
            (object obj, string description = "parameter")
        {
            var fmt = "." + L.f
                    + "Unsupported {0} : [{1}] " + "{2}";

            var msg = fmt.f(description, obj, obj.GetType().Name.Guillemet());

            return new NotSupportedException(msg);
        }

        public static NullReferenceException NullRef(string description)
        {
            return new NullReferenceException("{0} is NULL.".f(description));
        }


        public static MissingMemberException NoMember(string memberName, string message = "Member not found: “{0}”.")
            => new MissingMemberException(message.f(memberName));

        public static MissingMemberException NoMember<T>(string memberName, string message = "Member not found: ‹{0}› “{1}”.")
            => new MissingMemberException(message.f(typeof(T).Name, memberName));


        public static UnauthorizedAccessException Unauthorized(string message, Exception inner = null)
        {
            return new UnauthorizedAccessException(NewLine(message), inner);
        }


        private static string NewLine(string message, params object[] args)
        {
            return "." + L.f + message.f(args);
        }


        public static TimeoutException TimedOut(string message)
        {
            return new TimeoutException(NewLine(message));
        }


        public static ArgumentException BadArg(string argName, string shouldBeMsg)
        {
            return new ArgumentException($"Parameter “{argName}” {shouldBeMsg}.");
        }


        public static NotImplementedException Undone(string methodName, string msgLine2 = null)
        {
            var msg = "Method not done:  {0}()".f(methodName);
            if (!msgLine2.IsBlank()) msg += L.F + msgLine2;
            return new NotImplementedException(msg);
        }


        public static InvalidOperationException BadAct(string invalidOperationMsg)
        {
            return new InvalidOperationException(invalidOperationMsg);
        }


        public static FileNotFoundException MissingFile(string filePath)
        {
            return new FileNotFoundException("File not found: " + L.f + filePath);
        }



        public static RestServiceException Rest(string msg, IResponseShim resp)
        {
            return Error.Rest(msg, resp.Code, resp.Request.Method,
                                                resp.BaseUrl,
                                                resp.Request.Resource,
                                                resp.Error);
        }

        public static RestServiceException Rest(string msg,
                                                HttpStatusCode code,
                                                RestMethod method,
                                                string baseUrl,
                                                string resource,
                                                Exception inr = null)
        {
            return new RestServiceException(NewLine(msg), code, method, baseUrl, resource, inr);
        }



    }
}
