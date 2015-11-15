using System;
using ErrH.Tools.Drupal7Models.Fields;

namespace ErrH.Tools.Drupal7Models.FieldAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class D7FieldAttribute : Attribute
    {
        public string        FieldName  { get; }
        public D7FieldTypes  FieldType  { get; }


        public D7FieldAttribute(string fieldMachineName, D7FieldTypes fieldType)
        {
            FieldName = fieldMachineName;
            FieldType = fieldType;
        }
    }
}
