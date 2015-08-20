using System;
using System.Collections.Generic;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.Extensions;
using ErrH.Tools.Randomizers;
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


        protected override bool LoadList(string dataSourceUri, ref List<AppFolder> list)
        {
            var randomizr = new FakeFactory();

            for (int i = 0; i < 10; i++)
                list.Add(MockFolder(randomizr));

            return true;
        }

        protected override Func<AppFolder, object>
            GetKey => x => x.Alias;
    }
}
