using ErrH.Tools.Drupal7Models.Entities;
using ErrH.Tools.Drupal7Models.Fields;
using ErrH.Tools.Drupal7Models.FieldValues;

namespace ErrH.Tools.FileSynchronization
{
    public class SyncableFolderDto : D7NodeBase, ID7NodeRevision
    {
        public FieldUnd<UndTargetId>  field_users_ref          { get; set; }
        public FieldUnd<UndTargetId>  field_files_ref          { get; set; }
        public FieldUnd<UndValue>     field_currently_updating { get; set; }

        public int vid { get; set; }



        public SyncableFolderDto()
        {
            this.type = "app";
        }
    }
}
