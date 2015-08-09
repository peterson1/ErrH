using System.Linq;
using ErrH.Tools.Drupal7Models.Entities;
using ErrH.Tools.Drupal7Models.Fields;
using ErrH.UploaderApp.AppFileRepository;

namespace ErrH.UploaderApp.DTOs
{
    public class AppDtoRevision : AppDto, ID7NodeRevision
{
	public int vid { get; set; }


	public static AppDtoRevision From(AppItem orig)
	{
		return new AppDtoRevision {
			nid    =  orig.Nid,
			vid    =  orig.Vid,
			title  =  orig.Title,

			field_files_ref = und.TargetIds(orig.Files
							 .Select(x => x.Nid).ToArray()),

			field_users_ref = und.TargetIds(orig.Users.ToArray())
		};
	}
}
}
