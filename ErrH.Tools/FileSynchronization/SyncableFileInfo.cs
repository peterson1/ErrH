using System.ComponentModel;
using PropertyChanged;

namespace ErrH.Tools.FileSynchronization
{
    [ImplementPropertyChanged]
    public class SyncableFileInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string   Name      { get; set; }
        public long     Size      { get; set; }
        public string   Version   { get; set; }
        public string   SHA1      { get; set; }
        public string   UrlOrPath { get; set; }


        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            return this.GetHashCode() == obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            var key = Size + Version + SHA1;
            return key.GetHashCode();
        }
    }
}
