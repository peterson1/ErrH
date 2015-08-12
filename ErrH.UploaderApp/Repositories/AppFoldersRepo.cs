using System;
using System.Collections.Generic;
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



        public void Add(AppFolder appFoldr)
        {
            _list.Add(appFoldr);
            AppFolderAdded?.Invoke(this, EvtArg.AppDir(appFoldr));
        }


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
