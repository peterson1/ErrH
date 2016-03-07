namespace ErrH.RestClient.PCL45.Drupal7Models
{
    public interface ID7PostDTO
    {
        int      nid     { get; }
        int      uid     { get; }
        string   title   { get; }
        string   type    { get; }
    }


    public interface ID7PutDTO : ID7PostDTO
    {
        int   vid   { get; }
    }
}
