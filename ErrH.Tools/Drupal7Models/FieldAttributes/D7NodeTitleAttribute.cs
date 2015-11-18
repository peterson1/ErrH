using System;
using ErrH.Tools.Drupal7Models.Entities;
using ErrH.Tools.Drupal7Models.Fields;

namespace ErrH.Tools.Drupal7Models.FieldAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class D7NodeTitleAttribute : D7FieldAttribute
    {

        public D7NodeTitleAttribute() 
            : base(nameof(D7NodeBase.title), 
                   D7FieldTypes.DirectValue)
        {
        }
    }
}
