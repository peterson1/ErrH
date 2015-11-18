using System;
using ErrH.Tools.Drupal7Models.Fields;

namespace ErrH.Tools.Drupal7Models.FieldAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class D7Value2FieldAttribute : D7FieldAttribute
    {

        public D7Value2FieldAttribute(string fieldMachineName)
            : base(fieldMachineName, D7FieldTypes.CckField, true)
        {
        }
    }
}
