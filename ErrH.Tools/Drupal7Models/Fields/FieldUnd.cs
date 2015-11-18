using System.Collections.Generic;

namespace ErrH.Tools.Drupal7Models.Fields
{
    public class FieldUnd<T>
    {
        public List<T> und { get; set; } = new List<T>();
    }

    public struct UndValue
    {
        public object value;
        public object value2;
    }

    public struct UndTargetId { public int    target_id ;}
    public struct UndTermId   { public int    tid       ;}
    public struct UndFid
    {
        public int fid;
        public int display;
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
