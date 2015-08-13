using ErrH.AutofacShim;
using ErrH.Tools.FileSystemShims;
using ErrH.Tools.InversionOfControl;
using ErrH.UploaderApp.Repositories;
using ErrH.UploaderVVM.ViewModels;
using ErrH.WinTools.FileSystemTools;

namespace ErrH.UploaderVVM
{
    internal class IocResolver : TypeResolver
    {
        internal static ITypeResolver IoC = new IocResolver();


        protected override void RegisterTypes()
        {
            Singleton<IFileSystemShim, WindowsFsShim>();
            Singleton<AppFoldersRepo>();

            Register<MainWindow>();
            Register<MainWindowViewModel>();
            Register<AllAppFoldersViewModel>();
        }
    }
}
