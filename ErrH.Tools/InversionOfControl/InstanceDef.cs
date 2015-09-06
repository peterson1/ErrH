using System;

namespace ErrH.Tools.InversionOfControl
{
    public class InstanceDef
    {
        public Type  Interface       { get; set; }
        public Type  Implementation  { get; set; }
        public bool  IsSingleton     { get; set; }
    }
}
