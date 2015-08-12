using System.ComponentModel;
using ErrH.Tools.Loggers;
using ErrH.UploaderApp.AppFileRepository;
using PropertyChanged;

namespace ErrH.UploaderApp.Models
{
    [ImplementPropertyChanged]
    public class AppFile : LogSourceBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string    Name        { get; private  set; }
        public string    Difference  { get; internal set; }
        public string    location    { get; internal set; }
                                     
        public string    Sizes       { get; internal set; }
        public string    Versions    { get; internal set; }

        public VsRemote  Compared    { get; internal set; }
        public Sending   State       { get; internal set; }



        public AppFile(string fileName)
        {
            this.Name = fileName;
        }

        public AppFile(AppFileNode appF, VsRemote againstRemote)
        : this(appF.Name)
        {
            this.Compared = againstRemote;
        }

    }
}
