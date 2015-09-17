using ErrH.Uploader.Core.Models;
using ErrH.WpfTools.ViewModels;

namespace ErrH.Uploader.ViewModels.ContentVMs
{
    public class FolderVM : WorkspaceVmBase
    {
        public AppFolder Model { get; set; }

        public int     Nid    => Model.Nid;
        public string  Alias  => Model.Alias;
        public string  Path   => Model.Path;



        public FolderVM(AppFolder appFoldr)
        {
            Model     = appFoldr;
        }



        public override string 
            DisplayName => Model.Alias;
    }
}
