using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Input;
using ErrH.Tools.Extensions;
using ErrH.Wpf.net45.Commands;
using ErrH.Wpf.net45.Extensions;
using ErrH.WpfRestClient.net45.Configuration;
using ErrH.WpfRestClient.net45.Services;
using NLog;
using PropertyChanged;
using static CommandLine.Parser;

namespace ErrH.WpfRestClient.net45.ViewModels
{
    [ImplementPropertyChanged]
    public abstract class MainWindowVmBase<TCfg, TCmd> : ViewModelBase
        where TCfg : RestClientCfg, new()
        where TCmd : CommandLineOptions, new()
    {
        private static Logger _logr = LogManager.GetCurrentClassLogger();

        private      EventHandler _configLoaded;
        public event EventHandler  ConfigLoaded
        {
            add    { _configLoaded -= value; _configLoaded += value; }
            remove { _configLoaded -= value; }
        }

        public TCfg          Cfg           { get; }
        public TCmd          Cmd           { get; }
        public BinUpdaterVM  Updater       { get; }
        public string        UpdaterStatus { get; private set; }
        public ICommand      QuitAppCmd    { get; private set; }




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
            _configLoaded.Raise();
            OnConfigLoaded();

            view.DataContext = this;
            if (!Cmd.StartHidden) view.Show();


            //ServicePointManager.ServerCertificateValidationCallback
            //    += (a, b, c, d) => Validate(b);


            Updater = CreateBinUpdater();
            UpdaterStatus = Updater.Status;
            Updater.PropertyChanged += (s, f) =>
            {
                if (f.PropertyName == nameof(Updater.Status))
                    UpdaterStatus = Updater.Status;
            };
            //if (!Cmd.NoUpdates) Updater.StartChecking(Cfg.BinUpdater);

            D7PosterService.LaunchAsNeeded();

            QuitAppCmd = new TrappedCommand(_ =>
            {
                _logr.Info("Application.Shutdown by QuitAppCmd");
                Application.Current.Shutdown();
            });

            _logr.Info("“{0}” started.", Title);
        }

        protected virtual void OnConfigLoaded()
        {
        }

        //protected virtual bool Validate(X509Certificate x509cert)
        //{
        //    const string errHCert = "341E845315CF8CFF77428ABC4A0394E31133DB7C";
        //    const string repo1Cert = "68BD712DFC7529ED73D2E5E3F1A4EB5DFBA50164";

        //    var cert = x509cert as X509Certificate2;
        //    if (cert == null) return false;

        //    return cert.Thumbprint == Cfg.ServerThumb 
        //        || cert.Thumbprint == errHCert
        //        || cert.Thumbprint == repo1Cert;
        //}



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
