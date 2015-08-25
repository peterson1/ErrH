using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.ErrorConstructors;
using ErrH.Tools.Helpers;
using ErrH.UploaderApp;
using ErrH.UploaderApp.AppFileRepository;
using ErrH.UploaderApp.Models;
using ErrH.UploaderApp.Repositories;
using ErrH.WpfTools.ViewModels;

namespace ErrH.UploaderVVM.ViewModels
{
    public class FilesListVM : ListWorkspaceVMBase<AppFileViewModel>
    {
        private AppFolder            _app;
        private IRepository<AppFileNode> _remotes;


        public FilesListVM(IRepository<AppFileNode> filesRepo)
        {
            _remotes = ForwardLogs(filesRepo);
            _remotes.Loaded += (s, e) 
                => { RefreshVMList(); };
        }


        public override void SetIdentifier(object identifier)
        {
            base.SetIdentifier(identifier);

            _app = Cast.As<AppFolder>(identifier);
            DisplayName = _app.Alias;

            _remotes.Load(URL.repo_data_source, _app.Nid);
        }


        public override int HashCodeFor(object identifier)
        {
            var key = GetType().Name + Cast.As<AppFolder>(identifier).Nid;
            return key.GetHashCode();
        }


        protected override List<AppFileViewModel> DefineListItems()
            => _remotes.All.Select(x 
                => new AppFileViewModel(x)).ToList();
    }
}
