using System;
using ErrH.Tools.Drupal7Models.Fields;

namespace ErrH.Tools.Drupal7Models.FieldAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class D7NodeRefFieldAttribute : D7FieldAttribute
    {
        public D7NodeRefFieldAttribute(string fieldMachineName) 
            : base(fieldMachineName, D7FieldTypes.NodeReference)
        {
        }
    }
}
