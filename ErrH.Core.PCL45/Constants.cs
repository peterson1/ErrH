using System;

namespace ErrH.Core.PCL45
{
    public struct L
    {
        public static string f => Environment.NewLine;
        public static string F => L.f + L.f;
    }
}
