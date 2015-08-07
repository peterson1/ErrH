using System;
using System.Diagnostics;
using System.Reflection;

namespace ErrH.WinTools.ReflectionTools
{
    public class MethodNow
    {

        public static T CallerAttr<T>(int frameOffset = 2) where T : Attribute
        {
            var method = JumpBack(frameOffset);
            return Attr<T>(method);
        }


        public static MethodBase JumpBack(int frameOffset)
        {
            var stack = new StackTrace();
            var method = stack.GetFrame(frameOffset).GetMethod();
            return method;
        }


        public static T Attr<T>(MethodBase method) where T : Attribute
        {
            var attrs = method.GetCustomAttributes(typeof(T), true);

            if (attrs.Length == 0)
                throw new MissingMemberException(
                    $"No custom attributes ‹{typeof(T).Name}› found in method [{method.Name}].");

            return (T)attrs[0];

        }

    }
}
