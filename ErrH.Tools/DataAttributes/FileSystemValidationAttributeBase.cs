using ErrH.Tools.FileSystemShims;

namespace ErrH.Tools.DataAttributes
{
    public abstract class FileSystemValidationAttributeBase : ValidationAttributeBase
    {
        public IFileSystemShim FsShim = null;
    }
}
