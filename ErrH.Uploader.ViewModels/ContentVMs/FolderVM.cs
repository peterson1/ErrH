using ErrH.Uploader.Core.Models;
using ErrH.WpfTools.ViewModels;

namespace ErrH.Uploader.ViewModels.ContentVMs
{
    public class FolderVM : WorkspaceViewModelBase
    {
        public AppFolder Model { get; set; }

        public int     Nid    => Model.Nid;
        public string  Alias  => Model.Alias;
        public string  Path   => Model.Path;



        public FolderVM(AppFolder appFoldr, int listIndex)
        {
            Model     = appFoldr;
            ListIndex = listIndex;
        }



        public override string 
            DisplayName => Model.Alias;
    }
}
