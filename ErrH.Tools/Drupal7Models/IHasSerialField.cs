using ErrH.Tools.Drupal7Models.Fields;
using ErrH.Tools.Drupal7Models.FieldValues;

namespace ErrH.Tools.Drupal7Models
{
    public interface IHasSerialField
    {
        FieldUnd<UndValue>  field_serial  { get; set; }
    }
}
