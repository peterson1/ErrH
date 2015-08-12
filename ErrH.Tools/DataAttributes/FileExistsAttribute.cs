using ErrH.Tools.ErrorConstructors;
using ErrH.Tools.Extensions;
using ErrH.Tools.FileSystemShims;

namespace ErrH.Tools.DataAttributes
{
    public class FileExistsAttribute : ValidationAttributeBase
    {
        internal IFileSystemShim FsShim = null;


        public override bool TryValidate(string proprtyName, object value, out string invalidMsg)
        {
            if (value == null) goto ReturnTrue;
            var path = value.ToString();
            if (path.IsBlank()) goto ReturnTrue;

            Throw.IfNull(FsShim, "validator " + nameof(FsShim));

            if (!FsShim.IsFileFound(path))
            {
                invalidMsg = $"“{proprtyName}” does not exist as a file in {path}.";
                return false;
            }

        ReturnTrue:
            invalidMsg = string.Empty;
            return true;
        }
    }
}
