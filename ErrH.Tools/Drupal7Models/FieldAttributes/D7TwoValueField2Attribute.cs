using System;
using ErrH.Tools.Drupal7Models.Fields;

namespace ErrH.Tools.Drupal7Models.FieldAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class D7TwoValueField2Attribute : D7FieldAttribute
    {
        public D7TwoValueField2Attribute(string fieldMachineName) 
            : base(fieldMachineName, D7FieldTypes.CckField)
        {
            Has2Values = true;
            IsValue2 = true;
        }
    }
}
