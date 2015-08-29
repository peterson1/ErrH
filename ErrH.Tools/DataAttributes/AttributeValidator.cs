using System.Collections.Generic;
using ErrH.Tools.ErrorConstructors;
using ErrH.Tools.Extensions;
using ErrH.Tools.FileSystemShims;
using ErrH.Tools.Loggers;

namespace ErrH.Tools.DataAttributes
{
    internal class AttributeValidator : LogSourceBase
    {
        internal IFileSystemShim FsShim = null;



        internal string GetAllErrors<T>(T objWithAttributes)
        {
            var msgs = new List<string>();
            var props = objWithAttributes.GetType().GetProperties();
            foreach (var prop in props)
            {
                var msg = GetErrorMessage(objWithAttributes, prop.Name);
                if (!msg.IsBlank()) msgs.Add(msg);
            }

            if (msgs.Count == 0) return "";
            return string.Join(L.f, msgs);
        }



        internal string GetErrorMessage<T>(T objWithAttributes, string propertyName)
        {
            var typ = typeof(T);
            if (typ.Name == "object") throw Error.BadArg(
                nameof(objWithAttributes), "cannot be of type ‹object›");

            var msg = $"‹{typ.Name}› instance to validate should not be null.";
            if (objWithAttributes == null)
                return Warn_(msg, "Validation failed.", msg);

            var prop = typ.GetProperty(propertyName);
            msg = $"Missing property “{propertyName}” from ‹{typ.Name}›." 
                + L.f + "Try explicitly casting to a sub-class.";
            if (prop == null) return Warn_(msg, msg, msg);

            var atts = prop.GetCustomAttributes
                (typeof(ValidationAttributeBase), false);
            if ((atts?.Length ?? 0) == 0) return "";

            var msgs = new List<string>();
            var val = prop.GetValue(objWithAttributes,
                                     new object[] { });
            Trace_i($"‹{typ.Name}› “{prop.Name}” = {val}");

            foreach (var attrib in atts)
            {
                var att = attrib as ValidationAttributeBase;
                if (att != null)
                {
                    AttachFields(att);
                    var isValid = att.TryValidate(propertyName, val, out msg);
                    if (!isValid) msgs.Add(msg);
                }
            }

            if (msgs.Count == 0)
                return Trace_o_("", "Property validation :  Passed.");
            else
                return Warn_o_(string.Join(L.f, msgs), 
                        "Validation failed: " + msg);
        }


        private void AttachFields(ValidationAttributeBase att)
        {
            var fsValidation = att as FileSystemValidationAttributeBase;
            if (fsValidation != null)
            {
                fsValidation.FsShim = this.FsShim;
                return;
            }

            // add more attachments here
        }
    }

}
