using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;
using ErrH.Tools.Extensions;
using ErrH.Tools.InversionOfControl;
using ErrH.Tools.MvvmPattern;
using ErrH.WpfTools.CollectionShims;

namespace ErrH.WpfTools.ViewModels
{
    public abstract class MainWindowVMBase : ListWorkspaceVMBase<WorkspaceVmBase>
    {
        private      EventHandler _completelyLoaded;
        public event EventHandler  CompletelyLoaded
        {
            add    { _completelyLoaded -= value; _completelyLoaded += value; }
            remove { _completelyLoaded -= value; }
        }


        protected ITypeResolver           IoC         { get; }
        public    VmList<WorkspaceVmBase> Workspaces  { get; }
        public    VmList<ListItemVmBase>  StatusVMs   { get; }
        public    UserSessionVM           UserSession { get; private set; }



        public MainWindowVMBase(ITypeResolver resolvr)
        {
            IoC = resolvr;

            Workspaces = new VmList<WorkspaceVmBase>
                            (new List<WorkspaceVmBase>());

            StatusVMs = new VmList<ListItemVmBase>
                            (new List<ListItemVmBase>());

            Workspaces.CollectionChanged += this.OnWorkspacesChanged;

            CompletelyLoaded += (s, e) =>
            {
                UserSession = ForwardLogs(IoC.Resolve<UserSessionVM>());
                Refresh();
            };
        }




        void OnWorkspacesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (WorkspaceVmBase workspace in e.NewItems)
                    workspace.Closed += this.OnWorkspaceRequestClose;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (WorkspaceVmBase workspace in e.OldItems)
                    workspace.Closed -= this.OnWorkspaceRequestClose;
        }


        void OnWorkspaceRequestClose(object sender, EventArgs e)
        {
            WorkspaceVmBase workspace = sender as WorkspaceVmBase;
            workspace.Dispose();
            this.Workspaces.Remove(workspace);
        }


        protected virtual void SetActiveWorkspace(WorkspaceVmBase workspace)
        {
            Debug.Assert(this.Workspaces.Contains(workspace));

            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Workspaces);
            if (collectionView == null) return;

            var found = collectionView.MoveCurrentTo(workspace);
            if (!found)
                Warn_n($"{GetType().Name} : MainWindowVMBase.SetActiveWorkspace()", 
                        $"Workspace not found: “{workspace}”");
        }




        public void ShowSingleton<T>(object identifier, ITypeResolver resolvr)
            where T : WorkspaceVmBase
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
