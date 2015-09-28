using ErrH.Tools.Extensions;
using ErrH.Tools.MvvmPattern;

namespace ErrH.Tools.FileSynchronization
{
    public class RemoteVsLocalFile : ListItemVmBase
    {
        public string    Filename       { get; }
        public FileDiff  Comparison     { get; private set; }
        public string    OddProperty    { get; private set; }
        public string    PropertyDiffs  { get; private set; }
        public FileTask  NextStep       { get; private set; }
        public Target    Target         { get; private set; }
        public string    Status         { get; set; }

        public SyncableFileRemote  Remote { get; }
        public SyncableFileLocal   Local  { get; }


        public RemoteVsLocalFile(string filename,
                                 SyncableFileRemote remoteFile,
                                 SyncableFileLocal localFile,
                                 SyncDirection syncDirection)
        {
            Filename   = filename;
            Remote     = remoteFile;
            Local      = localFile;
            Status     = "Comparing...";
            Comparison = GetComparison(Remote, Local, syncDirection);
            Status     = "Idle.";
        }



        public void DoNext(Target target, FileTask nextStep)
        {
            NextStep = nextStep;
            Target = target;
        }


        private FileDiff GetComparison(SyncableFileBase remoteFile,
                                       SyncableFileBase localFile,
                                       SyncDirection syncDirection)
        {
            if (localFile == null && remoteFile == null)
            {
                DoNext(Target.Both, FileTask.Analyze);
                return FileDiff.Unavailable;
            }

            if (localFile == null)
            {
                if (syncDirection == SyncDirection.Upload)
                    DoNext(Target.Remote, FileTask.Delete);

                else if (syncDirection == SyncDirection.Download)
                    DoNext(Target.Local, FileTask.Create);

                return FileDiff.NotInLocal;
            }

            if (remoteFile == null)
            {
                if (syncDirection == SyncDirection.Upload)
                    DoNext(Target.Remote, FileTask.Create);

                else if (syncDirection == SyncDirection.Download)
                    DoNext(Target.Local, FileTask.Ignore);

                return FileDiff.NotInRemote;
            }

            if (remoteFile.Size != localFile.Size)
            {
                OddProperty = nameof(localFile.Size);
                PropertyDiffs = $"↑ {remoteFile.Size.KB()}{L.f}↓ {localFile.Size.KB()}";

                if (syncDirection == SyncDirection.Upload)
                    DoNext(Target.Remote, FileTask.Replace);

                else if (syncDirection == SyncDirection.Download)
                    DoNext(Target.Local, FileTask.Replace);

                return FileDiff.Changed;
            }

            if (remoteFile.Version != localFile.Version)
            {
                OddProperty = nameof(localFile.Version);
                PropertyDiffs = $"↑ “{remoteFile.Version}”{L.f}↓ “{localFile.Version}”";

                if (syncDirection == SyncDirection.Upload)
                    DoNext(Target.Remote, FileTask.Replace);

                else if (syncDirection == SyncDirection.Download)
                    DoNext(Target.Local, FileTask.Replace);

                return FileDiff.Changed;
            }

            if (remoteFile.SHA1 != localFile.SHA1)
            {
                OddProperty = nameof(localFile.SHA1);
                PropertyDiffs = $"↑ {remoteFile.SHA1}{L.f}↓ {localFile.SHA1}";

                if (syncDirection == SyncDirection.Upload)
                    DoNext(Target.Remote, FileTask.Replace);

                else if (syncDirection == SyncDirection.Download)
                    DoNext(Target.Local, FileTask.Replace);

                return FileDiff.Changed;
            }

            DoNext(Target.Both, FileTask.Ignore);
            return FileDiff.Same;
        }

    }
}
