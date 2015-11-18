using System.Collections.Generic;
using ErrH.Tools.Drupal7Models.FieldValues;

namespace ErrH.Tools.Drupal7Models.Fields
{
    public class FieldUnd<T> where T : IUndSomething
    {
        public List<T> und { get; set; } = new List<T>();
    }


    public enum D7FieldTypes
    {
        DirectValue,
        CckField,
        NodeReference,
        TermReference,
        FileReference
    }
}
