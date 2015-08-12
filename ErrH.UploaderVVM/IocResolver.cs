using ErrH.AutofacShim;
using ErrH.Tools.FileSystemShims;
using ErrH.WinTools.FileSystemTools;

namespace ErrH.UploaderVVM
{
    internal class IocResolver : TypeResolver
    {
        internal static IocResolver IoC = new IocResolver();


        protected override void RegisterTypes()
        {
            Singleton<IFileSystemShim, WindowsFsShim>();
        }
    }
}
