using System;
using System.Net;
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
                throw new ArgumentException("Could not determine name of host from base URL: " + baseUrl);

            ServicePointManager.ServerCertificateValidationCallback
                = ((sender, cert, chain, errors)
                    => cert.Subject.Contains(host));
        }
    }
}
