using System;
using System.Windows;
using ErrH.Wpf.net45.Extensions;
using ErrH.WpfRestClient.net45.Configuration;
using ErrH.WpfRestClient.net45.LogLayouts;
using ErrH.WpfRestClient.net45.Services;
using NLog;
using NLog.Config;
using static CommandLine.Parser;

namespace ErrH.WpfRestClient.net45.ViewModels
{
    public abstract class MainWindowVmBase<TCfg, TCmd> 
        where TCfg : RestClientCfg, new()
        where TCmd : CommandLineOptions, new()
    {
        protected static Logger        _logr = GetInitialLogger();

        protected RestClientCfg        _cfg;
        protected CommandLineOptions   _cmd;

        public BinUpdaterVM    Updater { get; }
        public abstract string Title   { get; }



        public MainWindowVmBase(Window view, StartupEventArgs e)
        {
            _cmd = CreateAndParseCmdArgs(e);

            _cfg = RestClientCfg.Load<TCfg>();
            if (_cfg == null)
            {
                view.Close();
                return;
            }

            view.DataContext = this;
            if (!_cmd.StartHidden) view.Show();

            Updater = CreateAndStartBinUpdater();
            D7PosterService.LaunchAsNeeded();

            _logr.Info("“{0}” started.", Title);
        }


        private CommandLineOptions CreateAndParseCmdArgs(StartupEventArgs e)
        {
            var opts = new TCmd();
            if (Default.ParseArguments(e.Args, opts)) return opts;

            _logr.Warn("Invalid cmd params: {0}", e);
            MessageBox.Show(opts.GetUsage());
            return opts;
        }


        private BinUpdaterVM CreateAndStartBinUpdater()
        {
            var upd8r = new BinUpdaterVM();

            upd8r.UpdatesInstalled += (s, e) =>
            {
                _logr.Info("Updates dowloaded and installed.");
                OnUpdatesInstalled();
            };

            upd8r.StartChecking(_cfg.BinUpdater);
            return upd8r;
        }


        protected virtual void OnUpdatesInstalled()
        {
            Application.Current.Relaunch
                (_cmd.StartHidden ? "-h" : null);
        }


        private static Logger GetInitialLogger()
        {
            ConfigurationItemFactory.Default.LayoutRenderers
                .RegisterDefinition("level-short", typeof(LogLevelInitialsLayoutRenderer));

            return LogManager.GetCurrentClassLogger();
        }
    }
}
