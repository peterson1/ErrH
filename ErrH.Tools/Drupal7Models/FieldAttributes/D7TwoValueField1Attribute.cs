using System;
using ErrH.Tools.Drupal7Models.Fields;

namespace ErrH.Tools.Drupal7Models.FieldAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class D7TwoValueField1Attribute : D7ValueFieldAttribute
    {

        public D7TwoValueField1Attribute(string fieldMachineName)
            : base(fieldMachineName)
        {
            Has2Values = true;
        }
    }
}
