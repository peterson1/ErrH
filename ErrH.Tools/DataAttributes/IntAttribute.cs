using System;
using ErrH.Tools.Extensions;

namespace ErrH.Tools.DataAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IntAttribute : ValidationAttributeBase
    {
        public int Min = -1;
        public int Max = -1;


        public override bool TryValidate(string proprtyName, object value, out string invalidMsg)
        {
            if (!value.ToString().IsNumeric())
            {
                invalidMsg = proprtyName + " should be numeric.";
                return false;
            }

            var num = value.ToString().ToInt();

            if (this.Min != -1 && num < this.Min)
            {
                invalidMsg = proprtyName + " should not be less than " + this.Min;
                return false;
            }

            if (this.Max != -1 && num > this.Max)
            {
                invalidMsg = proprtyName + " should not be greater than " + this.Max;
                return false;
            }

            invalidMsg = string.Empty;
            return true;
        }
    }
}
