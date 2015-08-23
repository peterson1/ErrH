using System.Collections.Generic;
using System.Linq;
using ErrH.Tools.ErrorConstructors;
using ErrH.Tools.Helpers;
using ErrH.UploaderApp.Models;
using ErrH.UploaderApp.Repositories;
using ErrH.WpfTools.ViewModels;

namespace ErrH.UploaderVVM.ViewModels
{
    public class FilesListVM : ListWorkspaceVMBase<AppFileViewModel>
    {
        private readonly IFilesRepo _repo;

        public AppFolder App { get; private set; }


        public FilesListVM(IFilesRepo repo)
        {
            _repo       = repo;
        }

        public override void SetIdentifier(object identifier)
        {
            base.SetIdentifier(identifier);

            App = Cast.As<AppFolder>(identifier);
            DisplayName = App.Alias;
            //_repo.Load("");
        }


        public override int HashCodeFor(object identifier)
        {
            var key = GetType().Name + Cast.As<AppFolder>(identifier).Nid;
            return key.GetHashCode();
        }


        protected override List<AppFileViewModel> DefineListItems()
            => _repo.FilesForApp(App.Nid).Select(x 
                => new AppFileViewModel(x)).ToList();
    }
}
