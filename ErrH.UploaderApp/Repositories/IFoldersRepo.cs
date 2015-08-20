using System;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.ScalarEventArgs;
using ErrH.UploaderApp.Models;

namespace ErrH.UploaderApp.Repositories
{
    public interface IFoldersRepo : IRepository<AppFolder>
    {
        event EventHandler<UrlEventArg> CertSelfSigned;
    }
}
