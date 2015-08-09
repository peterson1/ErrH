using ErrH.Tools.Drupal7Models.Entities;
using ErrH.Tools.Drupal7Models.Fields;

namespace ErrH.UploaderApp.DTOs
{
    public class AppDto : D7NodeBase
{
	public FieldUnd<UndTargetId>  field_users_ref  { get; set; }
	public FieldUnd<UndTargetId>  field_files_ref  { get; set; }

	public AppDto()	{ this.type = "app"; }
}
}
