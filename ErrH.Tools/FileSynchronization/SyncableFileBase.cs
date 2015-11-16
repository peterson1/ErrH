using System.ComponentModel;
using ErrH.Tools.Drupal7Models.Entities;
using PropertyChanged;

namespace ErrH.Tools.FileSynchronization
{
    [ImplementPropertyChanged]
    public abstract class SyncableFileBase : D7NodeBase
    {
        //public event PropertyChangedEventHandler PropertyChanged;

        public string   Name      { get; set; }
        public long     Size      { get; set; }
        public string   Version   { get; set; }
        public string   SHA1      { get; set; }


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
