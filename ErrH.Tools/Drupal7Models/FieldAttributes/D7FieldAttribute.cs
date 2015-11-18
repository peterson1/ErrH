using System;
using ErrH.Tools.Drupal7Models.Fields;
using ErrH.Tools.Extensions;

namespace ErrH.Tools.Drupal7Models.FieldAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class D7FieldAttribute : Attribute
    {
        public string        FieldName  { get; }
        public D7FieldTypes  FieldType  { get; }
        public bool          Has2Values { get; set; }
        public bool          IsValue2   { get; set; }


        public D7FieldAttribute(string fieldMachineName, D7FieldTypes fieldType)
        {
            FieldName = fieldMachineName;
            FieldType = fieldType;
        }



        public object GetValue1<T>(T item)// where T : ID7Node
        {
            foreach (var prop in item.GetType().PublicInstanceProps())
            {
                var att = prop.GetAttribute<D7FieldAttribute>(true);

                if (att != null && !att.IsValue2
                 && att.FieldName == this.FieldName)
                    return prop.GetValue(item, null);
            }
            return null;
        }
    }
}
