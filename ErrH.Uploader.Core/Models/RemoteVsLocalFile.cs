using ErrH.Tools.MvvmPattern;

namespace ErrH.Uploader.Core.Models
{
    public class RemoteVsLocalFile : ListItemVmBase
    {

        public string    Filename     { get; }
        public FileDiff  Comparison   { get; }
        public string    OddProperty  { get; private set; }
        public Action    NextStep     { get; private set; }
        public Target    Target       { get; private set; }
        public string    Status       { get; set; }

        public AppFileInfo Remote     { get; }
        public AppFileInfo Local      { get; }


        public RemoteVsLocalFile(string filename,
                                 AppFileInfo remoteFile,
                                 AppFileInfo localFile)
        {
            Filename   = filename;
            Remote     = remoteFile;
            Local      = localFile;
            Status     = "Comparing...";
            Comparison = GetComparison(remoteFile, localFile);
            Status     = "Idle.";
        }


        public void DoNext(Target target, Action nextStep)
        {
            NextStep = nextStep;
            Target = target;
        }


        private FileDiff GetComparison(AppFileInfo remoteFile,
                                       AppFileInfo localFile)
        {
            if (localFile == null && remoteFile == null)
            {
                DoNext(Target.Both, Action.Analyze);
                return FileDiff.Unavailable;
            }

            if (localFile == null)
            {
                DoNext(Target.Remote, Action.Delete);
                return FileDiff.NotInLocal;
            }

            if (remoteFile == null)
            {
                DoNext(Target.Remote, Action.Create);
                return FileDiff.NotInRemote;
            }

            if (remoteFile.Size != localFile.Size)
            {
                OddProperty = nameof(localFile.Size);
                DoNext(Target.Remote, Action.Replace);
                return FileDiff.Changed;
            }

            if (remoteFile.Version != localFile.Version)
            {
                OddProperty = nameof(localFile.Version);
                DoNext(Target.Remote, Action.Replace);
                return FileDiff.Changed;
            }

            if (remoteFile.SHA1 != localFile.SHA1)
            {
                OddProperty = nameof(localFile.SHA1);
                DoNext(Target.Remote, Action.Replace);
                return FileDiff.Changed;
            }

            DoNext(Target.Both, Action.Ignore);
            return FileDiff.Same;
        }

    }
}
