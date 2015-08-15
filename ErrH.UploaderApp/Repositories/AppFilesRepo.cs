using System.Collections.Generic;
using ErrH.Tools.Randomizers;
using ErrH.UploaderApp.Models;

namespace ErrH.UploaderApp.Repositories
{
    public class AppFilesRepo
    {

        public List<AppFile> FilesForApp(int nid)
        {
            var randomizr = new FakeFactory();
            var list = new List<AppFile>();

            for (int i = 0; i < 10; i++)
                list.Add(MockFile(randomizr));

            return list;
        }


        private AppFile MockFile(FakeFactory random)
        {
            return new AppFile(random.Filename)
            {
                
            };
        }




    }
}
