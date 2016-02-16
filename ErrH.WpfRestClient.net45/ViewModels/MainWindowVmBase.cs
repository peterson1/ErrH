using System.Windows;
using ErrH.WpfRestClient.net45.Configuration;
using ErrH.WpfRestClient.net45.LogLayouts;
using NLog;
using NLog.Config;
using static CommandLine.Parser;

namespace ErrH.WpfRestClient.net45.ViewModels
{
    public abstract class MainWindowVmBase<TCfg, TCmd> 
        where TCfg : RestClientCfg, new()
        where TCmd : CommandLineOptions, new()
    {
        protected static RestClientCfg _cfg  = RestClientCfg.Load<TCfg>();
        protected static Logger        _logr = GetInitialLogger();
        protected CommandLineOptions   _cmd;

        public BinUpdaterVM    Updater { get; } = new BinUpdaterVM();
        public abstract string Title   { get; }


        public MainWindowVmBase(Window view, StartupEventArgs e)
        {
            var _cmd = new TCmd();

            if (!Default.ParseArguments(e.Args, _cmd))
            {
                _logr.Warn("Invalid cmd params: {0}", e);
                MessageBox.Show(_cmd.GetUsage());
            }
            
            view.DataContext = this;

            _logr.Info("“{0}” started.", Title);

            if (_cfg == null)
            {
                view.Close();
                return;
            }
            if (!_cmd.StartHidden) view.Show();
        }


        private static Logger GetInitialLogger()
        {
            ConfigurationItemFactory.Default.LayoutRenderers
                .RegisterDefinition("level-short", typeof(LogLevelInitialsLayoutRenderer));

            return LogManager.GetCurrentClassLogger();
        }
    }
}
