using ErrH.BinUpdater.Core.Configuration;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.Extensions;
using ErrH.Tools.FileSynchronization;
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



        public FoldersTabVM(IRepository<SyncableFolderInfo> foldersRepo, IConfigFile cfgFile)
        {
            DisplayName  = "Local Folders";
            _repo        = ForwardLogs(foldersRepo);
            MainList     = new VmList<SyncableFolderInfo>();

            cfgFile.CertSelfSigned += (s, e) 
                => { Ssl.AllowSelfSignedFrom(e.Url); };
        }


        protected override void OnRefresh()
        {
            if (!_repo.Load(ThisApp.Folder.FullName))
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
