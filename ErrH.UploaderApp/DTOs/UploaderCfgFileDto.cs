using System.Collections.Generic;
using ErrH.Configuration;

namespace ErrH.UploaderApp.DTOs
{
public class UploaderCfgFileDto : ConfigFileDto
{
	public List<LocalAppCfgDto> local_apps { get; set; }
}
}
