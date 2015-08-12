using System;
using ErrH.Tools.Extensions;

namespace ErrH.Tools.DataAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredAttribute : ValidationAttributeBase
    {
        public override bool TryValidate(string proprtyName, object value, out string invalidMsg)
        {

            if (value == null)
            {
                invalidMsg = $"“{proprtyName}” should not be ‹NULL›.";
                return false;
            }

            if (value.ToString().IsBlank())
            {
                invalidMsg = $"“{proprtyName}” should not be ‹BLANK›.";
                return false;
            }


            invalidMsg = string.Empty;
            return true;
        }
    }
}
