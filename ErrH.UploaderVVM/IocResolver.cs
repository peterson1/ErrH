using ErrH.AutofacShim;
using ErrH.JsonNetShim;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.FileSystemShims;
using ErrH.Tools.InversionOfControl;
using ErrH.Tools.Serialization;
using ErrH.UploaderApp;
using ErrH.UploaderApp.Models;
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
            Singleton<ISerializer, JsonNetSerializer>();
            
            //Singleton<IFoldersRepo, MockFoldersRepo>();
            Singleton<IFoldersRepo, LocalFoldersRepo>();
            Singleton<UploaderCfgFile>();


            Register<IFilesRepo, MockFilesRepo>();

            Register<MainWindow>();
            Register<MainWindowVM>();
            Register<AllAppFoldersVM>();
        }
    }
}
