using System;
using ErrH.Tools.Extensions;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;
using ErrH.Tools.InversionOfControl;

namespace ErrH.WpfTools.ViewModels
{
    public abstract class MainWindowVMBase : WorkspaceViewModelBase
    {
        public event EventHandler CompletelyLoaded;

        //private ReadOnlyCollection<CommandViewModel>         _navigations;
        private ObservableCollection<WorkspaceViewModelBase> _workspaces;


        // Returns a read-only list of commands 
        // that the UI can display and execute.
        //public ReadOnlyCollection<CommandViewModel> Navigations
        //{
        //    get {
        //        if (_navigations == null) _navigations = new ReadOnlyCollection
        //            <CommandViewModel>(this.DefineNavigations());
        //        return _navigations;
        //    }
        //}
        //protected abstract List<CommandViewModel> DefineNavigations();



        /// <summary>
        /// Returns the collection of available workspaces to display.
        /// A 'workspace' is a ViewModel that can request to be closed.
        /// </summary>
        public ObservableCollection<WorkspaceViewModelBase> Workspaces
        {
            get {
                if (_workspaces == null)
                {
                    _workspaces = new ObservableCollection<WorkspaceViewModelBase>();
                    _workspaces.CollectionChanged += this.OnWorkspacesChanged;
                }
                return _workspaces;
            }
        }




        void OnWorkspacesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (WorkspaceViewModelBase workspace in e.NewItems)
                    workspace.RequestClose += this.OnWorkspaceRequestClose;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (WorkspaceViewModelBase workspace in e.OldItems)
                    workspace.RequestClose -= this.OnWorkspaceRequestClose;
        }


        void OnWorkspaceRequestClose(object sender, EventArgs e)
        {
            WorkspaceViewModelBase workspace = sender as WorkspaceViewModelBase;
            workspace.Dispose();
            this.Workspaces.Remove(workspace);
        }


        protected void SetActiveWorkspace(WorkspaceViewModelBase workspace)
        {
            Debug.Assert(this.Workspaces.Contains(workspace));

            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Workspaces);
            if (collectionView != null)
                collectionView.MoveCurrentTo(workspace);
        }




        //receive type and ID only
        //protected void ShowSingleton<T>(T wrkspace) 
        //    where T: WorkspaceViewModelBase
        //{
        //    var match = Workspaces.Where(x => x is T)
        //        .FirstOrDefault(x => x.DisplayName 
        //            == wrkspace.DisplayName) as T;

        //    if (match == null)
        //        Workspaces.Add(wrkspace);
        //    else
        //        wrkspace = match;

        //    SetActiveWorkspace(wrkspace);
        //}



        protected void ShowSingleton<T>(object identifier, ITypeResolver resolvr)
            where T : WorkspaceViewModelBase
        {
            T wrkspce = (T)Workspaces.Where(x => x is T)
                .FirstOrDefault(x => x.GetHashCode()
                    == x.HashCodeFor(identifier));

            if (wrkspce == null)
            {
                try {
                    wrkspce = ForwardLogs(resolvr.Resolve<T>());
                }
                catch (Exception ex)
                {
                    Error_n("Error in ShowSingleton<T>()", 
                        $"Unable to create instance of ‹{typeof(T).Name}›." 
                        + L.F + ex.Details(false, false));
                    return;
                }
                wrkspce.SetIdentifier(identifier);
                Workspaces.Add(wrkspce);
            }

            SetActiveWorkspace(wrkspce);
        }



        public MainWindowVMBase SetCloseHandler(Window window)
        {
            EventHandler handlr = null;
            handlr = delegate
            {
                this.RequestClose -= handlr;
                window.Close();
            };
            this.RequestClose += handlr;
            return this;
        }




        public MainWindowVMBase SetLoadCompleteHandler()
        {
            Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.ApplicationIdle, new Action(() =>
            {
                CompletelyLoaded?.Invoke(this, EventArgs.Empty);
            }));
            return this;
        }
    }
}
