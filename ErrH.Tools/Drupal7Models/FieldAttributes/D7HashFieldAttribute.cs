using System;
using System.Linq;
using System.Reflection;
using ErrH.Tools.Extensions;

namespace ErrH.Tools.Drupal7Models.FieldAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class D7HashFieldAttribute : D7ValueFieldAttribute
    {
        public PropertyInfo  ModelProperty { get; set; }
        public PropertyInfo  D7Property    { get; set; }

        public D7HashFieldAttribute(string fieldMachineName) 
            : base(fieldMachineName)
        {
            IsHash = true;
        }


        public static D7HashFieldAttribute FindIn<T>()
        {
            foreach (var prop in typeof(T).PublicInstanceProps())
            {
                var att = prop.GetAttribute<D7HashFieldAttribute>();
                if (att != null)
                {
                    att.ModelProperty = prop;

                    var dtoAtt = typeof(T).GetAttribute<D7NodeDtoAttribute>();
                    att.D7Property = dtoAtt?.DtoType.PublicInstanceProps()
                                        .FirstOrDefault(x => x.Name == att.FieldName);
                    return att;
                }
            }
            return null;
        }
    }
}
