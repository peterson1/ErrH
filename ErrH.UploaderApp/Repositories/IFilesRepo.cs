using System;
using System.Collections.Generic;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.ScalarEventArgs;
using ErrH.UploaderApp.Models;

namespace ErrH.UploaderApp.Repositories
{
    public interface IFilesRepo : IRepository<AppFile>
    {
        event EventHandler<UserEventArg> LoggedIn;

        List<AppFile> FilesForApp(int nid);
    }
}
