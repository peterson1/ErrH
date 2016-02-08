using System;
using System.ComponentModel;
using System.Threading.Tasks;
using ErrH.D7Poster.WPF.DataAccess;
using ErrH.Tools.Extensions;
using ErrH.Wpf.net45.BinUpdater;
using NLog;

namespace ErrH.D7Poster.WPF.ViewModels
{
    class BinUpdaterVM : BinUpdaterBase
    {
        private static Logger _logr = LogManager.GetCurrentClassLogger();

        private D7Client _d7c = new D7Client();


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
            try {
                return _d7c.PersistentGet<T>(resource, cfg);
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
