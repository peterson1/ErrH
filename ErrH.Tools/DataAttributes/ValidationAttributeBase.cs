using System;

namespace ErrH.Tools.DataAttributes
{
    public abstract class ValidationAttributeBase : Attribute
    {
        public abstract bool TryValidate
            (string proprtyName, object value, out string invalidMsg);

    }
}
