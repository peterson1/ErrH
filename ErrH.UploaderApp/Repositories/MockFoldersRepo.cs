using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.Extensions;
using ErrH.Tools.Randomizers;
using ErrH.Tools.ScalarEventArgs;
using ErrH.UploaderApp.Models;

namespace ErrH.UploaderApp.Repositories
{
    public class MockFoldersRepo : ListRepoBase<AppFolder>
    {
        private AppFolder MockFolder(FakeFactory random)
            => new AppFolder {
                Nid   = random.Int(1, 999),
                Alias = random.Namespace,
                Path  = random.FolderPath
            };


        protected override List<AppFolder> LoadList(object[] args)
        {
            var randomizr = new FakeFactory();
            List<AppFolder> list = null;

            for (int i = 0; i < 10; i++)
                list.Add(MockFolder(randomizr));

            return list;
        }

        protected override Func<AppFolder, object>
            GetKey => x => x.Alias;
    }
}
