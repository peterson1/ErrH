namespace ErrH.Tools.Drupal7Models.Fields
{
    public class FileRef_Clean
    {
        public CleanResourceId file { get; set; }

        public static FileRef_Clean New(int fid)
        {
            return new FileRef_Clean
            {
                file = CleanResourceId.File(fid)
            };
        }
    }
}
