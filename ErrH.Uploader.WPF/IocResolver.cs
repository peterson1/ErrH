using ErrH.AutofacShim;
using ErrH.Drupal7Client;
using ErrH.JsonNetShim;
using ErrH.Tools.Authentication;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.Drupal7Models;
using ErrH.Tools.FileSystemShims;
using ErrH.Tools.InversionOfControl;
using ErrH.Tools.Serialization;
using ErrH.Uploader.Core.Configuration;
using ErrH.Uploader.Core.Models;
using ErrH.Uploader.Core.Nodes;
using ErrH.Uploader.Core.Services;
using ErrH.Uploader.DataAccess;
using ErrH.Uploader.ViewModels;
using ErrH.Uploader.ViewModels.ContentVMs;
using ErrH.Uploader.ViewModels.NavigationVMs;
using ErrH.WinTools.FileSystemTools;
using ErrH.WpfTools.ViewModels;

namespace ErrH.Uploader.WPF
{
    internal class IocResolver : TypeResolver
    {
        internal static ITypeResolver IoC = new IocResolver();


        protected override void RegisterTypes()
        {
            //Singleton<ITypeResolver>(IocResolver.IoC);
            Singleton<IFileSystemShim, WindowsFsShim>();
            Singleton<ISerializer, JsonNetSerializer>();
            Singleton<ISessionClient, ID7Client, D7ServicesClient>();
            Singleton<IConfigFile, UploaderCfgFile>();

            Singleton<IRepository<AppFolder>, LocalFoldersRepo>();
            //Singleton<IRepository<AppFolder>, FakeFoldersRepo>();
            Register<IRepository<AppFileNode>, RemoteFilesRepo>();
            //Register<IRepository<AppFileNode>, FakeFilesRepo>();

            Register<MainWindow>();
            Register<MainWindowVM>();
            Register<FoldersTabVM>();
            Register<FilesTabVM2>();

            Register<UserSessionVM>();

            Register<AppFileGrouper>();
            Register<LocalFileSeeker>();
        }
    }
}
