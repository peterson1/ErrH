using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using ErrH.Tools.Extensions;
using ErrH.Tools.InversionOfControl;
using ErrH.WpfTools.CollectionShims;
using ErrH.WpfTools.Commands;
using PropertyChanged;

namespace ErrH.WpfTools.ViewModels
{
    [ImplementPropertyChanged]
    public abstract class MainWindowVmBase : WorkspaceVmBase
    {
        private      EventHandler _completelyLoaded;
        public event EventHandler  CompletelyLoaded
        {
            add    { _completelyLoaded -= value; _completelyLoaded += value; }
            remove { _completelyLoaded -= value; }
        }


        public ITypeResolver            IoC         { get; set; }
        public UserSessionVM            UserSession { get; }
        public VmList<WorkspaceVmBase>  NaviTabs    { get; }
        public VmList<WorkspaceVmBase>  MainTabs    { get; }
        public VmList<WorkspaceVmBase>  OtherTabs   { get; }

        public bool  DetailsAvailable  { get; set; }


        public RelayCommand ShutdownCommand { get; } = CreateShutdownCommand();




        private static RelayCommand CreateShutdownCommand()
            => new RelayCommand(x => Application.Current.Shutdown());



        public MainWindowVmBase(UserSessionVM userSessionVM)
        {
            NaviTabs    = new VmList<WorkspaceVmBase>();
            MainTabs    = new VmList<WorkspaceVmBase>();
            OtherTabs   = new VmList<WorkspaceVmBase>();
            UserSession = ForwardLogs(userSessionVM);

            MainTabs.CollectionChanged += OnWorkspacesChanged;

            NaviTabs.ItemPicked += ResetDetailsState;
            MainTabs.ItemPicked += ResetDetailsState;


            CompletelyLoaded += (s, e) => { Refresh(); };
        }

        private void ResetDetailsState(object sender, Tools.ScalarEventArgs.EArg<WorkspaceVmBase> e)
        {
            DetailsAvailable = false;
            //RaisePropertyChanged(nameof(DetailsAvailable));
        }




        //protected override void OnRefresh()
        //{
        //    base.OnRefresh();

        //}


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
            this.MainTabs.Remove(workspace);
        }


        public virtual void SetActiveWorkspace(WorkspaceVmBase workspace)
        {
            //Debug.Assert(this.MainTabs.Contains(workspace));

            //ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.MainTabs);
            //if (collectionView == null) return;

            //var found = collectionView.MoveCurrentTo(workspace);
            if (!MainTabs.MakeCurrent(workspace))
                Warn_n($"{GetType().Name} : MainWindowVMBase.SetActiveWorkspace()", 
                        $"Workspace not found: “{workspace}”");
        }




        public void ShowSingleton<T>(object identifier, ITypeResolver resolvr)
            where T : WorkspaceVmBase
        {
            T wrkspce = (T)MainTabs.Where(x => x is T)
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
                MainTabs.Add(wrkspce);
                wrkspce.SetIdentifier(identifier);
                wrkspce.Refresh();
            }

            SetActiveWorkspace(wrkspce);
            return;
        }



        public void ShowTogether( WorkspaceVmBase naviVm
                                , WorkspaceVmBase mainVm
                                , WorkspaceVmBase othrVm) 
        {
            naviVm.IsSelectedChanged += (s, e) =>
            {
                if (e.Value)
                {
                    MainTabs.MakeCurrent(mainVm);
                    OtherTabs.MakeCurrent(othrVm);
                }
            };
            mainVm.IsSelectedChanged += (s, e) =>
            {
                if (e.Value)
                {
                    NaviTabs.MakeCurrent(naviVm);
                    OtherTabs.MakeCurrent(othrVm);
                }
            };
            othrVm.IsSelectedChanged += (s, e) =>
            {
                if (e.Value)
                {
                    NaviTabs.MakeCurrent(naviVm);
                    MainTabs.MakeCurrent(mainVm);
                }
            };
        }



        public MainWindowVmBase SetCloseHandler(Window window)
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




        public MainWindowVmBase SetLoadCompleteHandler()
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
