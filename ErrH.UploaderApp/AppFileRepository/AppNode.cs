using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ErrH.UploaderApp.AppFileRepository
{
public class AppNode
{
    internal AppFileRepo Repo { get; set; }

    public int        Nid     { get; set; }
    public int        Vid     { get; set; }
    public string     Title   { get; set; }
    public List<int>  Users   { get; set; }

    public ReadOnlyCollection<AppFileNode> 
        Files { get { return this.Files(); }}
}
}
