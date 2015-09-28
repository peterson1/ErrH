using ErrH.AutofacShim;
using ErrH.BinUpdater.Core;
using ErrH.BinUpdater.Core.Configuration;
using ErrH.BinUpdater.DataAccess;
using ErrH.Drupal7Client;
using ErrH.Drupal7FileUpdater;
using ErrH.JsonNetShim;
using ErrH.Tools.Authentication;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.Drupal7Models;
using ErrH.Tools.FileSynchronization;
using ErrH.Tools.FileSystemShims;
using ErrH.Tools.InversionOfControl;
using ErrH.Tools.Serialization;
using ErrH.Uploader.DataAccess;
using ErrH.Uploader.ViewModels;
using ErrH.Uploader.ViewModels.ContentVMs;
using ErrH.Uploader.ViewModels.NavigationVMs;
using ErrH.WinTools.FileSystemTools;

namespace ErrH.Uploader.WPF
{
    internal class IocResolver : TypeResolver
    {
        internal static ITypeResolver IoC = new IocResolver();


        protected override void RegisterTypes()
        {
            Singleton<IFileSystemShim, WindowsFsShim>();
            Singleton<ISerializer, JsonNetSerializer>();
            Singleton<ISessionClient, ID7Client, D7ServicesClient>();
            Singleton<IConfigFile, UploaderCfgFile>();

            Singleton<IRepository<SyncableFolderInfo>, LocalFoldersRepo>();
            Register<IRepository<SyncableFileRemote>, RemoteFilesRepo>();
            Register<IFileSynchronizer, D7FileSynchronizer>();

            Register<MainWindow>();
            Register<MainWindowVM>();
            Register<FoldersTabVM>();
            Register<FilesTabVM2>();

            Register<AppFileGrouper>();
            Register<LocalFileSeeker>();
        }
    }
}
