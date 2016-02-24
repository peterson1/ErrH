using System.Net;
using System.Security.Cryptography.X509Certificates;
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
    public abstract class MainWindowVmBase<TCfg, TCmd> : ViewModelBase
        where TCfg : RestClientCfg, new()
        where TCmd : CommandLineOptions, new()
    {
        private static Logger _logr = LogManager.GetCurrentClassLogger();

        public TCfg            Cfg     { get; }
        public TCmd            Cmd     { get; }
        public BinUpdaterVM    Updater { get; }
        public abstract string Title   { get; }



        public MainWindowVmBase(Window view, StartupEventArgs e)
            : base()
        {
            Cmd = CreateAndParseCmdArgs(e) as TCmd;

            Cfg = RestClientCfg.Load<TCfg>();
            if (Cfg == null)
            {
                view.Close();
                return;
            }

            view.DataContext = this;
            if (!Cmd.StartHidden) view.Show();


            //todo: instead of returning true, check server thumb
            ServicePointManager.ServerCertificateValidationCallback
                += (a, b, c, d) => Validate(b);


            Updater = CreateBinUpdater();
            if (!Cmd.NoUpdates) Updater.StartChecking(Cfg.BinUpdater);

            D7PosterService.LaunchAsNeeded();

            _logr.Info("“{0}” started.", Title);
        }



        protected virtual bool Validate(X509Certificate x509cert)
        {
            var cert = x509cert as X509Certificate2;
            if (cert == null) return false;

            return cert.Thumbprint == Cfg.ServerThumb;
        }



        private CommandLineOptions CreateAndParseCmdArgs(StartupEventArgs e)
        {
            var opts = new TCmd();
            if (Default.ParseArguments(e.Args, opts)) return opts;

            _logr.Warn("Invalid cmd params: {0}", e);
            MessageBox.Show(opts.GetUsage());
            return opts;
        }


        private BinUpdaterVM CreateBinUpdater()
        {
            var upd8r = new BinUpdaterVM();

            upd8r.UpdatesInstalled += (s, e) =>
            {
                _logr.Info("Updates dowloaded and installed.");
                OnUpdatesInstalled();
            };

            //upd8r.StartChecking(_cfg.BinUpdater);
            return upd8r;
        }


        protected virtual void OnUpdatesInstalled()
        {
            Application.Current.Relaunch
                (Cmd.StartHidden ? "-h" : null);
        }


        //private static Logger GetInitialLogger()
        //{
        //    ConfigurationItemFactory.Default.LayoutRenderers
        //        .RegisterDefinition("level-short", typeof(LogLevelInitialsLayoutRenderer));

        //    return LogManager.GetCurrentClassLogger();
        //}
    }
}
