using System;
using ErrH.Tools.Drupal7Models.Fields;

namespace ErrH.Tools.Drupal7Models.FieldAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class D7ValueFieldAttribute : D7FieldAttribute
    {

        public D7ValueFieldAttribute(string fieldMachineName) 
            : base(fieldMachineName, D7FieldTypes.CckField)
        {
        }
    }
}
