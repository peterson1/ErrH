using ErrH.BinUpdater.Core.Configuration;
using ErrH.WpfTools.ViewModels;

namespace ErrH.BinUpdater.ViewModels
{
    public class UpdaterSessionVM : UserSessionVM
    {
        public UpdaterSessionVM(BinUpdaterCfgFile authKeyFile) 
            : base(authKeyFile) { }
    }
}
