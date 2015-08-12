
namespace ErrH.UploaderApp.AppFileRepository
{

public class AppFileNode
{
    internal AppNode App { get; set; }

    public int     Nid      { get; set; }
    public int     Vid      { get; set; }
    public string  Name     { get; set; }

    public string  Version  { get; set; }
    public long    Size     { get; set; }
    public string  SHA1     { get; set; }
                                
    public int     Fid      { get; set; }


}
}
