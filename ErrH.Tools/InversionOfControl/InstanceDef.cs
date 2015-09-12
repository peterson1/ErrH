using System;

namespace ErrH.Tools.InversionOfControl
{
    public class InstanceDef
    {
        public Type  Interface1      { get; set; }
        public Type  Interface2      { get; set; }
        public Type  Implementation  { get; set; }
        public bool  IsSingleton     { get; set; }
    }
}
