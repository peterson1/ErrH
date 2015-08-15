using System.Collections.Generic;
using System.Linq;
using ErrH.UploaderApp.Models;
using ErrH.UploaderApp.Repositories;
using ErrH.WpfTools.ViewModels;

namespace ErrH.UploaderVVM.ViewModels
{
    public class FilesListViewModel : ListWorkspaceViewModelBase<AppFileViewModel>
    {
        private readonly AppFilesRepo _repo;

        public AppFolder App { get; }



        public FilesListViewModel(AppFolder appFoldr, AppFilesRepo repo)
        {
            _repo       = repo;
            App         = appFoldr;
            DisplayName = App.Alias;
        }


        protected override List<AppFileViewModel> DefineListItems()
            => _repo.FilesForApp(App.Nid).Select(x 
                => new AppFileViewModel(x)).ToList();
    }
}
