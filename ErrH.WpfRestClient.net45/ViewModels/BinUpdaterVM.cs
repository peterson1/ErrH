using System;
using System.ComponentModel;
using System.Threading.Tasks;
using ErrH.Tools.Extensions;
using ErrH.Wpf.net45.BinUpdater;
using ErrH.WpfRestClient.net45.DataAccess;
using NLog;
using PropertyChanged;

namespace ErrH.WpfRestClient.net45.ViewModels
{
    [ImplementPropertyChanged]
    public class BinUpdaterVM : BinUpdaterBase
    {
        private static Logger _logr = LogManager.GetCurrentClassLogger();



        public BinUpdaterVM()
        {
            ((INotifyPropertyChanged)this).PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(Status))
                    _logr.Info(Status);
            };
        }

        protected override Task<T> ApiGet<T>(string resource, BinUpdaterKey cfg)
        {
            var d7c = new D7Client(cfg.BaseURL, cfg.Username, cfg.Password);
            try {
                return d7c.PersistentGet<T>(resource);
            }
            catch (Exception ex)
            {
                _logr.Error(ex.Details());
                return null;
            }
        }

        protected override bool LogError(string message)
        {
            _logr.Error(message);
            return false;
        }
    }
}
