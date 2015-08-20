using System;
using System.Collections.Generic;
using System.Linq;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.Randomizers;
using ErrH.UploaderApp.Models;

namespace ErrH.UploaderApp.Repositories
{
    public class MockFilesRepo : ListRepoBase<AppFile>, IFilesRepo
    {


        public List<AppFile> FilesForApp(int nid) 
            => All.ToList();



        private AppFile MockFile(FakeFactory random)
            => new AppFile(random.Filename) { };



        protected override bool LoadList(string dataSourceUri, ref List<AppFile> list)
        {
            var randomizr = new FakeFactory();

            for (int i = 0; i < 10; i++)
                list.Add(MockFile(randomizr));

            return true;
        }

        protected override Func<AppFile, object>
            GetKey => x => x.Name;
    }
}
