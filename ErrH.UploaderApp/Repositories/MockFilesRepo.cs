using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.ErrorConstructors;
using ErrH.Tools.Randomizers;
using ErrH.UploaderApp.Models;

namespace ErrH.UploaderApp.Repositories
{
    public class MockFilesRepo : ListRepoBase<AppFileDiff>
    {

        //public List<AppFile> FilesForApp(int nid) 
        //    => All.ToList();



        private AppFileDiff MockFile(FakeFactory random)
            => new AppFileDiff(random.Filename) { };





        public new async Task<bool> Load(params object[] args)
        {
            var randomizr = new FakeFactory();
            _list.Clear();

            for (int i = 0; i < 10; i++)
                _list.Add(MockFile(randomizr));

            await TaskEx.Delay(1000 * 10);
            Fire_Loaded();
            return true;
        }



        protected override List<AppFileDiff> LoadList(object[] args)
        {
            throw Error.BadAct("In an implementation of ISlowRepository<T>, method LoadList() should not be called.");
        }


        protected override Func<AppFileDiff, object>
            GetKey => x => x.Name;

    }
}
