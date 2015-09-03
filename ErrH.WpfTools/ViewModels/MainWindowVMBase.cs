using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;
using ErrH.Tools.Extensions;
using ErrH.Tools.InversionOfControl;
using ErrH.WpfTools.CollectionShims;

namespace ErrH.WpfTools.ViewModels
{
    public abstract class MainWindowVMBase : WorkspaceViewModelBase
    {
        private      EventHandler _completelyLoaded;
        public event EventHandler CompletelyLoaded
        {
            add    { _completelyLoaded -= value; _completelyLoaded += value; }
            remove { _completelyLoaded -= value; }
        }


        private WorkspacesList _workspaces;
        public  WorkspacesList  Workspaces
        {
            get
            {
                if (_workspaces != null) return _workspaces;
                _workspaces = new WorkspacesList(this);
                _workspaces.CollectionChanged += this.OnWorkspacesChanged;
                return _workspaces;
            }
        }


        public ITypeResolver IoC { get; set; }


        void OnWorkspacesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (WorkspaceViewModelBase workspace in e.NewItems)
                    workspace.Closed += this.OnWorkspaceRequestClose;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (WorkspaceViewModelBase workspace in e.OldItems)
                    workspace.Closed -= this.OnWorkspaceRequestClose;
        }


        void OnWorkspaceRequestClose(object sender, EventArgs e)
        {
            WorkspaceViewModelBase workspace = sender as WorkspaceViewModelBase;
            workspace.Dispose();
            this.Workspaces.Remove(workspace);
        }


        protected virtual void SetActiveWorkspace(WorkspaceViewModelBase workspace)
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



        public void ShowSingleton<T>(object identifier, ITypeResolver resolvr)
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
                Workspaces.Add(wrkspce);
                wrkspce.SetIdentifier(identifier);
                wrkspce.Refresh();
            }

            SetActiveWorkspace(wrkspce);
            return;
        }



        public MainWindowVMBase SetCloseHandler(Window window)
        {
            EventHandler handlr = null;
            handlr = delegate
            {
                this.Closed -= handlr;
                window.Close();
            };
            this.Closed += handlr;
            return this;
        }




        public MainWindowVMBase SetLoadCompleteHandler()
        {
            Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.ApplicationIdle, new Action(() =>
            {
                _completelyLoaded?.Invoke(this, EventArgs.Empty);
            }));
            return this;
        }
    }
}
