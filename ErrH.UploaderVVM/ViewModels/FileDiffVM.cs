using System;
using System.Windows.Input;
using ErrH.Tools.Extensions;
using ErrH.Tools.FileSystemShims;
using ErrH.UploaderApp;
using ErrH.UploaderApp.AppFileRepository;
using ErrH.WpfTools.Commands;
using ErrH.WpfTools.ViewModels;
using PropertyChanged;

namespace ErrH.UploaderVVM.ViewModels
{
    [ImplementPropertyChanged]
    public class FileDiffVM : ViewModelBase
    {
        private AppFileNode _remote;
        private FileShim    _local;


        public string Name => _remote?.Name ?? _local.Name;

        public string    Difference  { get; internal set; }
        public string    Location    { get; internal set; }
                                     
        public string    Sizes       { get; internal set; }
        public string    Versions    { get; internal set; }

        public VsRemote  Compared    { get; internal set; }
        public Sending   State       { get; internal set; }


        public override string DisplayName => Name;


        public ICommand Recompare 
            => new RelayCommand(x => RunCompare());


        public FileDiffVM(AppFileNode remote, FileShim local)
        {
            _remote = remote;
            _local = local;

        }


        public void RunCompare()
        {
            Compared = VsRemote.Checking;
            Location = "remote :" + L.f + "local :";

            CompareAgainstRemote();
            CompareAgainstLocal();
            if (_remote == null || _local == null) return;


            if (_local.Size != _remote.Size)
            {
                Compared = VsRemote.Changed;
                Difference = "different sizes";
                return;
            }

            Compared = (_local.SHA1 == _remote.SHA1)
                     ? VsRemote.Same : VsRemote.Changed;

            Difference = (Compared == VsRemote.Same)
                       ? "‹no diff›" : "different hashes";
        }




        private void CompareAgainstLocal()
        {
            Sizes += L.f;
            Versions += L.f;

            if (_local == null)
            {
                Compared = VsRemote.NotInLocal;
                Difference = "not in local";
            }
            else
            {
                Sizes += _local.Size.KB();
                Versions += " v." + _local.Version;
            }
        }


        private void CompareAgainstRemote()
        {
            if (_remote == null)
            {
                Compared = VsRemote.NotInRemote;
                Difference = "not in remote";
            }
            else
            {
                Sizes = _remote.Size.KB();
                Versions = " v." + _remote.Version;
            }
        }
    }
}
