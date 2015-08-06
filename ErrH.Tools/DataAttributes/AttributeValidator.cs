using ErrH.Tools.Extensions;
using ErrH.Tools.Loggers;

namespace ErrH.Tools.DataAttributes
{
    public class AttributeValidator : LogSourceBase
    {

        public bool IsValid(object objWithAttributes)
        {
            if (objWithAttributes == null)
                return Warn_n("Validation failed.", "Object should not be null.");

            var typ = objWithAttributes.GetType();
            var typNme = objWithAttributes.GetType().Name;

            foreach (var prop in typ.GetProperties())
            {
                var val = prop.GetValue(objWithAttributes, new object[] { });

                foreach (var att in prop.GetCustomAttributes
                                (typeof(ValidationAttributeBase), false))
                {
                    bool isValid; string msg;
                    var dta = att as ValidationAttributeBase;
                    if (dta != null)
                    {
                        Trace_i($"‹{typNme}› “{prop.Name}” = {val}");
                        isValid = dta.TryValidate(prop.Name, val, out msg);

                        if (isValid) return Trace_o("Property validation :  Passed.");
                        else return Warn_o("Validation failed: " + msg);
                    }
                }
            }
            return true;
        }
    }


    public static class ObjectValidatorExtension
    {
        public static bool ValidateTo(this object objWithAttributes, ILogSource logr)
        {
            var validatr = logr.ForwardLogs(new AttributeValidator());
            return validatr.IsValid(objWithAttributes);
        }
    }
}
