using ErrH.Tools.ErrorConstructors;
using ErrH.Tools.Extensions;

namespace ErrH.Tools.DataAttributes
{
    public class FolderExistsAttribute : FileSystemValidationAttributeBase
    {

        public override bool TryValidate(string proprtyName, object value, out string invalidMsg)
        {
            if (value == null) goto ReturnTrue;
            var path = value.ToString();
            if (path.IsBlank()) goto ReturnTrue;

            Throw.IfNull(FsShim, "validator " + nameof(FsShim));

            if (!FsShim.IsFolderFound(path))
            {
                invalidMsg = $"“{proprtyName}” does not exist as a folder in {path}.";
                return false;
            }

        ReturnTrue:
            invalidMsg = string.Empty;
            return true;
        }
    }
}
