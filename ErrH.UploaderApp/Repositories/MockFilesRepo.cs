using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.ErrorConstructors;
using ErrH.Tools.Extensions;
using ErrH.Tools.Randomizers;
using ErrH.Tools.ScalarEventArgs;
using ErrH.UploaderApp.AppFileRepository;
using ErrH.UploaderApp.Models;

namespace ErrH.UploaderApp.Repositories
{
    public class MockFilesRepo : ListRepoBase<AppFileDiffs>
    {
        public event EventHandler<UserEventArg> LoggedIn;


        //public List<AppFile> FilesForApp(int nid) 
        //    => All.ToList();



        private AppFileDiffs MockFile(FakeFactory random)
            => new AppFileDiffs(random.Filename) { };





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



        protected override List<AppFileDiffs> LoadList(object[] args)
        {
            throw Error.BadAct("In an implementation of ISlowRepository<T>, method LoadList() should not be called.");
        }


        protected override Func<AppFileDiffs, object>
            GetKey => x => x.Name;

    }
}
