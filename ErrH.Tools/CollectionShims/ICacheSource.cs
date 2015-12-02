namespace ErrH.Tools.CollectionShims
{
    public interface ICacheSource
    {
        bool  ClearCache    (string filter = "*");
        bool  HasCache      (string filter = "*");
        bool  UseCachedFile { get; set; }
    }
}
