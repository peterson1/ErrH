namespace ErrH.Tools.Drupal7Models.Entities
{
    public interface ID7Node
    {
        int nid { get; set; }
        int uid { get; set; }

        string title { get; set; }
        string type { get; set; }
    }
}
