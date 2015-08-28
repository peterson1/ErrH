using ErrH.AutofacShim;
using ErrH.Configuration;
using ErrH.Drupal7Client;
using ErrH.JsonNetShim;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.Drupal7Models;
using ErrH.Tools.FileSystemShims;
using ErrH.Tools.InversionOfControl;
using ErrH.Tools.Serialization;
using ErrH.UploaderApp;
using ErrH.UploaderApp.AppFileRepository;
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
            Singleton<IFileSystemShim       , WindowsFsShim>();
            Singleton<ISerializer           , JsonNetSerializer>();
            Singleton<ID7Client             , D7ServicesClient>();
            Singleton<IRepository<AppFolder>, LocalFoldersRepo>();
            Singleton<IConfigFile           , UploaderCfgFile>();

            Register<IRepository<AppFileNode>, RemoteFilesRepo>();

            Register<MainWindow>();
            Register<MainWindowVM>();
            //Register<FoldersListVM>();
            Register<SlowFoldersWVM>();
            //Register<FilesListVM>();
            Register<SlowFilesWVM>();
        }
    }
}
