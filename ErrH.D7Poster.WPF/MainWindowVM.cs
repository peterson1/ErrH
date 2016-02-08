using System.Collections.ObjectModel;
using ErrH.D7Poster.WPF.Configuration;
using ErrH.D7Poster.WPF.ViewModels;
using ErrH.Wpf.net45.Extensions;
using NLog;

namespace ErrH.D7Poster.WPF
{
    class MainWindowVM
    {
        private static SettingsCfg _cfg = SettingsCfg.Load<SettingsCfg>();
        private static Logger _logr = LogManager.GetCurrentClassLogger();

        public string Title => "D7 Poster";
        public ObservableCollection<TargetFolderVM> Targets { get; } = new ObservableCollection<TargetFolderVM>();
        public BinUpdaterVM Updater { get; } = new BinUpdaterVM();



        public MainWindowVM()
        {
            InitializeUpdater();

            foreach (var tgt in _cfg.Targets)
                Targets.Add(new TargetFolderVM(tgt));
        }


        //[Conditional("RELEASE")]
        private void InitializeUpdater()
        {
            Updater.UpdatesInstalled += (s, e)
                => App.Current.Relaunch();

            Updater.StartChecking(_cfg.BinUpdater);
        }
    }
}
