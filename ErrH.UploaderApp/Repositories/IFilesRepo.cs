using System.Collections.Generic;
using ErrH.Tools.CollectionShims;
using ErrH.UploaderApp.Models;

namespace ErrH.UploaderApp.Repositories
{
    public interface IFilesRepo : IRepository<AppFile>
    {
        List<AppFile> FilesForApp(int nid);
    }
}
