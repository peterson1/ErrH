using System;
using System.IO;
using static System.Environment;

namespace ErrH.Wpf.net45.Extensions
{
    public static class SpecialFolderExtensions
    {
        public static string Dir(this SpecialFolder specialF)
            => Environment.GetFolderPath(specialF);

        public static string Dir(this SpecialFolder specialF, string subFoldr)
            => Path.Combine(specialF.Dir(), subFoldr);
    }
}
