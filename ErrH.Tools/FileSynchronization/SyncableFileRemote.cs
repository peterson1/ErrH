namespace ErrH.Tools.FileSynchronization
{
    public class SyncableFileRemote : SyncableFileBase
    {
        //later: apply DataError validations here
        public int  Nid  { get; set; }
        public int  Vid  { get; set; }
                         
        public int  Fid  { get; set; }
    }
}
