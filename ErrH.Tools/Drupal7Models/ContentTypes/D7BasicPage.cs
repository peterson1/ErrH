using ErrH.Tools.Drupal7Models.Entities;
using ErrH.Tools.Drupal7Models.Fields;

namespace ErrH.Tools.Drupal7Models.ContentTypes
{
    public class D7BasicPage : D7NodeBase
    {

        public FieldUnd<UndValue> body { get; set; }


        public D7BasicPage()
        {
            type = "page";
        }
    }
}
