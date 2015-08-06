using System.Collections.Generic;

namespace ErrH.Tools.Drupal7Shim.Fields
{
    public class FieldUnd<T>
    {
        public List<T> und { get; set; }
    }

    public struct UndValue { public object value; }
    public struct UndTargetId { public int target_id; }
    public struct UndFid
    {
        public int fid;
        public int display;
    }
}
