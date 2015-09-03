using System.ComponentModel;
using PropertyChanged;

namespace ErrH.Uploader.Core.Models
{
    [ImplementPropertyChanged]
    public class RemoteVsLocalFile : INotifyPropertyChanged
    {
        private bool _hasChanges = false;

        public event PropertyChangedEventHandler PropertyChanged;


        public string  Filename     { get; }
        public string  OddProperty  { get; private set; }


        private AppFileInfo _remote;
        public  AppFileInfo  Remote
        {
            get { return _remote; }
            set
            {
                _remote = value;
                _remote.PropertyChanged += EchoEvent;
            }
        }

        private AppFileInfo _local;
        public  AppFileInfo  Local
        {
            get { return _local; }
            set
            {
                _local = value;
                _local.PropertyChanged += EchoEvent;
            }
        }



        public RemoteVsLocalFile(string filename)
        {
            Filename = filename;
        }



        public FileDiff Compare
        {
            get
            {
                if (Local == null && Remote == null)
                    return FileDiff.Unavailable;

                if (Local == null)
                    return FileDiff.NotInLocal;

                if (Remote == null)
                    return FileDiff.NotInRemote;

                if (!_hasChanges)
                    return FileDiff.Unavailable;


                if (Remote.Size != Local.Size)
                {
                    OddProperty = nameof(Local.Size);
                    return FileDiff.Changed;
                }


                if (Remote.Version != Local.Version)
                {
                    OddProperty = nameof(Local.Version);
                    return FileDiff.Changed;
                }


                if (Remote.SHA1 != Local.SHA1)
                {
                    OddProperty = nameof(Local.SHA1);
                    return FileDiff.Changed;
                }


                return FileDiff.Same;
            }
        }


        private void EchoEvent(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(sender, e);
            if (!_hasChanges) _hasChanges = true;
        }
    }
}
