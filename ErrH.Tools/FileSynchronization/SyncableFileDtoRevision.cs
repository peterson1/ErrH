using ErrH.Tools.Drupal7Models.Entities;

namespace ErrH.Tools.FileSynchronization
{
    public class SyncableFileDtoRevision : SyncableFileDto, ID7NodeRevision
    {
        public int vid { get; set; }



        public SyncableFileDtoRevision() : base()
        {
        }

        public SyncableFileDtoRevision(RemoteVsLocalFile inf, int fileID) 
            : base(inf.Local, fileID)
        {
            nid = inf.Remote.Nid;
            vid = inf.Remote.Vid;
        }
    }
}
