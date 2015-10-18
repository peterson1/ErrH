using System.Net;
using System.Security;

namespace ErrH.WinTools.Extensions
{
    public static class SecureStringExtensions
    {
        public static string Decrypt(this SecureString secureStr)
            => new NetworkCredential(string.Empty, secureStr).Password;
    }
}
