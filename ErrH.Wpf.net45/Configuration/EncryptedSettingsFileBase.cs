using System;
using System.IO;
using ErrH.Tools.Extensions;
using ErrH.Wpf.net45.Cryptographers;

namespace ErrH.Wpf.net45.Configuration
{
    public abstract class EncryptedSettingsFileBase : SettingsFileBase
    {
        private const string PREFIX = "tmpCfgEnc";

        protected override string ReadSettingsFile()
        {
            var contnt = base.ReadSettingsFile();
            if (contnt.IsBlank()) return "";

            DeleteOldConfigs();
            var pwd = GetPassword();

            if (contnt.Trim().Substring(0, 1) == "{")
            {
                var encryptd = AESThenHMAC.SimpleEncryptWithPassword(contnt, pwd);

                RewriteFile(encryptd);

                return contnt;
            }

            return AESThenHMAC.SimpleDecryptWithPassword(contnt, pwd);
        }


        private void DeleteOldConfigs()
        {
            var dir   = new DirectoryInfo(Folder);
            foreach (var tmpFile in dir.EnumerateFiles($"{PREFIX}*.*"))
            {
                try   { tmpFile.Delete(); }
                catch { }
            }
        }

        private void RewriteFile(string encryptd)
        {
            var realPath = Path.Combine(Folder, Filename);
            var tempFile = $"{PREFIX}_{DateTime.Now.Ticks}_{Filename}";

            File.Move(realPath, tempFile);
            File.WriteAllText(realPath, encryptd);
            File.SetAttributes(realPath, FileAttributes.Hidden);
        }

        private string GetPassword() => this.GetType().FullName.SHA1();
    }
}
