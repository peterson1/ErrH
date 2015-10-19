using ErrH.BinUpdater.Core.Configuration;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.Extensions;
using ErrH.Tools.FileSynchronization;
using ErrH.Uploader.DataAccess;
using ErrH.Uploader.DataAccess.Configuration;
using ErrH.WinTools.NetworkTools;
using ErrH.WinTools.ReflectionTools;
using ErrH.WpfTools.CollectionShims;
using ErrH.WpfTools.ViewModels;

namespace ErrH.Uploader.ViewModels.NavigationVMs
{
    public class FoldersTabVM : WorkspaceVmBase
    {
        private IRepository<SyncableFolderInfo> _repo;


        public VmList<SyncableFolderInfo> MainList { get; }



        public FoldersTabVM(IRepository<SyncableFolderInfo> foldersRepo, BinUploaderCfgFile cfgFile)
        {
            DisplayName  = "Local Folders";
            _repo        = ForwardLogs(foldersRepo);
            MainList     = new VmList<SyncableFolderInfo>();
        }


        protected override void OnRefresh()
        {
            if (!_repo.Load(Cfg.BinUploader))
            {
                Error_n("Failed to load Folders repo.", "");
                return;
            }

            MainList.Clear();
            _repo.All.ForEach(x => MainList.Add(x));
            MainList.SelectOne(0);
        }

    }
}
