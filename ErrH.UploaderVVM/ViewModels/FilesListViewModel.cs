using System.Collections.Generic;
using System.Linq;
using ErrH.UploaderApp.Models;
using ErrH.UploaderApp.Repositories;
using ErrH.WpfTools.ViewModels;

namespace ErrH.UploaderVVM.ViewModels
{
    public class FilesListViewModel : ListWorkspaceVMBase<AppFileViewModel>
    {
        private readonly IFilesRepo _repo;

        public AppFolder App { get; }



        public FilesListViewModel(AppFolder appFoldr, IFilesRepo repo)
        {
            _repo       = repo;
            App         = appFoldr;
            DisplayName = App.Alias;

            _repo.Load("");
        }


        protected override List<AppFileViewModel> DefineListItems()
            => _repo.FilesForApp(App.Nid).Select(x 
                => new AppFileViewModel(x)).ToList();
    }
}
