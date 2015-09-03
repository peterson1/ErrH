using System.Collections.Generic;
using ErrH.WpfTools.ViewModels;

namespace ErrH.WpfTools.CollectionShims
{
    public class WorkspacesList : Observables<WorkspaceViewModelBase>
    {
        private MainWindowVMBase _parentWindow;

        public WorkspacesList(MainWindowVMBase parentWindow) : base(new List<WorkspaceViewModelBase>())
        {
            _parentWindow = parentWindow;
        }

        public new void Add(WorkspaceViewModelBase workspace)
        {
            workspace.ParentWindow = _parentWindow;
            base.Add(workspace);
        }
        
    }
}
