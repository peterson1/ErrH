using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using ErrH.Tools.Extensions;
using ErrH.Tools.InversionOfControl;

namespace ErrH.WpfTools.ViewModels
{
    public abstract class MainWindowViewModelBase : WorkspaceViewModelBase
    {
        private ReadOnlyCollection<CommandViewModel>         _navigations;
        private ObservableCollection<WorkspaceViewModelBase> _workspaces;


        /// <summary>
        /// Returns a read-only list of commands 
        /// that the UI can display and execute.
        /// </summary>
        public ReadOnlyCollection<CommandViewModel> Navigations
        {
            get {
                if (_navigations == null) _navigations = new ReadOnlyCollection
                    <CommandViewModel>(this.DefineNavigations());
                return _navigations;
            }
        }
        protected abstract List<CommandViewModel> DefineNavigations();



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


        /// <summary>
        /// Displays the workspace of the specified type.
        /// If no instance yet, creates it first.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resolvr"></param>
        protected void ShowSingleton<T>(ITypeResolver 
            resolvr) where T : WorkspaceViewModelBase
        {
            var wrkspce = Workspaces.FirstOrDefault(x => x is T);
            if (wrkspce == null)
            {
                try { wrkspce = resolvr.Resolve<T>(); }
                catch (Exception ex) {
                    MessageBox.Show(ex.Message(true, false)); }
                if (wrkspce == null) return;
                Workspaces.Add(wrkspce);
            }
            SetActiveWorkspace(wrkspce);
        }



        protected void ShowSingleton<T>(T wrkspace) 
            where T: WorkspaceViewModelBase
        {
            var match = Workspaces.Where(x => x is T)
                .FirstOrDefault(x => x.DisplayName 
                    == wrkspace.DisplayName) as T;

            if (match == null)
                Workspaces.Add(wrkspace);
            else
                wrkspace = match;

            SetActiveWorkspace(wrkspace);
        }


    }
}
