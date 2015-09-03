using ErrH.Uploader.Core.Models;
using ErrH.WpfTools.ViewModels;

namespace ErrH.Uploader.ViewModels.ContentVMs
{
    public class FolderVM : WorkspaceViewModelBase
    {
        private AppFolder _model;

        public int     Nid    => _model.Nid;
        public string  Alias  => _model.Alias;
        public string  Path   => _model.Path;



        public FolderVM(AppFolder appFoldr, int listIndex)
        {
            _model    = appFoldr;
            ListIndex = listIndex;
        }



        public override string 
            DisplayName => _model.Alias;
    }
}
