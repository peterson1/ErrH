using System;
using System.Collections.Generic;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.Randomizers;
using ErrH.Uploader.Core.Models;

namespace ErrH.Uploader.DataAccess
{
    public class FakeFoldersRepo : ListRepoBase<AppFolder>
    {

        protected override List<AppFolder> LoadList(object[] args)
        {
            var fke = new FakeFactory();
            var list = new List<AppFolder>();

            for (int i = 0; i < fke.Int(5,10); i++)
            {
                list.Add(new AppFolder
                {
                    Alias = fke.ProperNoun,
                    Nid   = fke.Int(1, 1000),
                    Path  = fke.FolderPath
                });
            }

            return list;
        }


        protected override Func<AppFolder, object> 
            GetKey => x => x.Nid;
    }
}
