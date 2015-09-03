using System.Collections.Generic;

namespace ErrH.Uploader.Core.DTOs
{
    public class UploaderCfgFileDto : ConfigFileDto
    {
        public List<LocalAppCfgDto> local_apps { get; set; }
    }
}
