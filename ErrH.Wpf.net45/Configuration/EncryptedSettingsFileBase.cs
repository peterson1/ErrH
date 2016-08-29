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
            var pwd = ComposePassword();

            if (contnt.Trim().Substring(0, 1) == "{")
            {
                var encryptd = AESThenHMAC.SimpleEncryptWithPassword(contnt, pwd);
                RewriteLockedFile(encryptd);
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


        private void RewriteLockedFile(string content)
        {
            var tempFile = $"{PREFIX}_{DateTime.Now.Ticks}_{Filename}";
            File.Move(ActualFilePath(), tempFile);
            RewriteColdFile(content);
        }


        private void RewriteColdFile(string content)
        {
            var fPath = ActualFilePath();
            File.WriteAllText  (fPath, content);
            File.SetAttributes (fPath, FileAttributes.Hidden);
        }


        public void     DecryptSavedFile () => RewriteLockedFile(this.ReadSettingsFile());
        private string  ComposePassword  () => this.GetType().FullName.SHA1();
        private string  ActualFilePath   () => Path.Combine(Folder, Filename);
    }
}
