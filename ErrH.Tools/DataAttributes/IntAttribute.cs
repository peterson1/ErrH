using System;
using ErrH.Tools.Extensions;

namespace ErrH.Tools.DataAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IntAttribute : RequiredAttribute
    {
        public int Min = int.MinValue;
        public int Max = int.MinValue;


        public override bool TryValidate(string proprtyName, object value, out string invalidMsg)
        {
            if (!base.TryValidate(proprtyName, value, out invalidMsg)) return false;

            if (!value.ToString().IsNumeric())
            {
                invalidMsg = $"{proprtyName} “{value}” should be numeric.";
                return false;
            }

            var num = value.ToString().ToInt();

            if (this.Min != int.MinValue && num < this.Min)
            {
                invalidMsg = $"“{proprtyName}” should not be less than {Min}.";
                return false;
            }

            if (this.Max != int.MinValue && num > this.Max)
            {
                invalidMsg = $"“{proprtyName}” should not be greater than {Max}.";
                return false;
            }

            invalidMsg = string.Empty;
            return true;
        }
    }
}
