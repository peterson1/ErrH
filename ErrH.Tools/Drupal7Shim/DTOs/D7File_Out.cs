using System;
using ErrH.Tools.Drupal7Shim.Entities;
using ErrH.Tools.Extensions;
using ErrH.Tools.FileSystemShims;
using ErrH.Tools.Loggers;

namespace ErrH.Tools.Drupal7Shim.DTOs
{
    public class D7File_Out
    {
        public D7File file { get; set; }


        public D7File_Out() { }


        public D7File_Out(FileShim fShim,
                          string serverFoldr,
                          bool isPrivate)
        {
            //if (!fShim.Found) Throw.Missing(fShim);
            var defLevl = fShim.DefaultLevel;
            fShim.DefaultLevel = L4j.Off;

            var fPath = isPrivate ? "private://" : "public://";
            fPath = fPath.Slash(serverFoldr).Slash(fShim.Name);

            this.file = new D7File
            {
                file = fShim.ToBase64,
                filename = fShim.Name,
                filepath = fPath,
                //status   = Constants.FILE_STATUS_PERMANENT,
                //filemime = "application/x-msdos-program"
                //filemime = 149
            };

            fShim.DefaultLevel = defLevl;
        }

    }



    public static class D7FileDtoExtensions
    {

        public static byte[] Bytes(this D7File d7FileDto)
        {
            return Convert.FromBase64String(d7FileDto.file);
        }

    }
}
