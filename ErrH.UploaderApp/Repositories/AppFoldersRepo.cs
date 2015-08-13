using System;
using System.Collections.Generic;
using ErrH.Tools.DataAttributes;
using ErrH.Tools.ErrorConstructors;
using ErrH.Tools.Extensions;
using ErrH.Tools.FileSystemShims;
using ErrH.Tools.Randomizers;
using ErrH.UploaderApp.EventArguments;
using ErrH.UploaderApp.Models;

namespace ErrH.UploaderApp.Repositories
{
    public class AppFoldersRepo
    {
        public event EventHandler<AppFolderEventArg> AppFolderAdded;


        private readonly List<AppFolder> _list;



        public AppFoldersRepo()
        {
            var randomizr = new FakeFactory();

            _list = new List<AppFolder>();

            for (int i = 0; i < 10; i++)
                _list.Add(MockFolder(randomizr));
        }



        public bool Add(AppFolder newAppFoldr, IFileSystemShim fsShim)
        {
            if (!DataError.IsBlank(newAppFoldr))
                throw Error.BadAct("Attempted to Save() invalid AppFolder.");

            if (this.Contains(newAppFoldr)) return false;

            _list.Add(newAppFoldr);
            AppFolderAdded?.Invoke(this, EvtArg.AppDir(newAppFoldr));
            return true;
        }



        public bool Contains(AppFolder appFoldr)
            => _list.Has(x => x.Alias == appFoldr.Alias);


        public List<AppFolder> All => new List<AppFolder>(_list);


        private AppFolder MockFolder(FakeFactory random)
        {
            return new AppFolder
            {
                Nid = random.Int(1, 999),
                Alias = random.Namespace,
                Path = random.FolderPath
            };
        }
    }
}
