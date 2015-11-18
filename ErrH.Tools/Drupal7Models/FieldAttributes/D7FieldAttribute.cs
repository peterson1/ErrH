using System;
using ErrH.Tools.Drupal7Models.Fields;

namespace ErrH.Tools.Drupal7Models.FieldAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class D7FieldAttribute : Attribute
    {
        public string        FieldName  { get; }
        public D7FieldTypes  FieldType  { get; }
        public bool          IsValue2   { get; }


        public D7FieldAttribute(string fieldMachineName, D7FieldTypes fieldType, bool isValue2)
        {
            FieldName = fieldMachineName;
            FieldType = fieldType;
            IsValue2  = isValue2;
        }
    }
}
