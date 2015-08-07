using System.Net;
using ErrH.Tools.ErrorConstructors;
using ErrH.Tools.Extensions;

namespace ErrH.WinTools.NetworkTools
{
    public class Ssl
    {
        //from http://stackoverflow.com/a/6613434/3973863
        public static void AllowSelfSignedFrom(string baseUrl)
        {
            var host = baseUrl.Between("//", ":", true);
            if (host.IsBlank())
                Throw.BadData("Invalid BaseURL: " + baseUrl);

            ServicePointManager.ServerCertificateValidationCallback
                = ((sender, cert, chain, errors)
                    => cert.Subject.Contains(host));
        }
    }
}
