using System;
using System.Security.Cryptography.X509Certificates;
using ErrH.Tools.Extensions;
using ErrH.Tools.Loggers;
using ErrH.WinTools.ReflectionTools;

namespace ErrH.WinTools.Cryptography
{
    public class X509
    {
        public static bool AddCertificate(string fileName, ILogSource logr)
        {
            logr.Trace_i($"Installing {fileName}...");
            var file = Self.Folder.File(fileName);
            if (!file.Found) return logr.Error_o("File not found");

            try
            {
                var cert = new X509Certificate2(file.Path);
                var stor = new X509Store(StoreName.TrustedPeople, StoreLocation.CurrentUser);
                stor.Open(OpenFlags.ReadWrite);
                stor.Add(cert);
                stor.Close();
                return logr.Trace_o("Successfully installed bundled certificate.");
            }
            catch (Exception ex)
            {
                return logr.Error_o("Error encountered." + L.F + ex.Details(false, false));
            }
        }
    }
}
