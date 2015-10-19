using ErrH.Tools.Authentication;
using ErrH.Tools.FileSystemShims;
using ErrH.Tools.Serialization;
using PropertyChanged;

namespace ErrH.BinUpdater.Core.Configuration
{
    [ImplementPropertyChanged]
    public class BinUpdaterCfgFile : LoginCfgFile
    {
        public int  AppNid       { get; set; }
        public int  IntervalMins { get; set; }



        public BinUpdaterCfgFile(IFileSystemShim fileSystemShim, ISerializer serializer) 
            : base(fileSystemShim, serializer) { }


        public override bool ReadFrom(string fileName)
        {
            var typd = ReadAs<BinUpdaterCfgFile>(fileName);
            if (typd == null) return false;

            AppNid       = typd.AppNid;
            IntervalMins = typd.IntervalMins;
            return true;
        }
    }
}
