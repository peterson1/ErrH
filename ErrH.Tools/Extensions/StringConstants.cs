using System;

namespace ErrH.Tools.Extensions
{
    public struct L
    {
        public static string f { get { return Environment.NewLine; } }
        public static string F { get { return L.f + L.f; } }
    }
}
