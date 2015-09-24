using System;
using System.Collections.Generic;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.FileSynchronization;
using ErrH.Tools.Randomizers;

namespace ErrH.Uploader.DataAccess
{
    public class FakeFoldersRepo : ListRepoBase<SyncableFolderInfo>
    {

        protected override List<SyncableFolderInfo> LoadList(object[] args)
        {
            Info_n("Loading list of folders...", args[0]);

            var fke = new FakeFactory();
            var list = new List<SyncableFolderInfo>();

            for (int i = 0; i < fke.Int(5,10); i++)
            {
                list.Add(new SyncableFolderInfo
                {
                    Alias = fke.ProperNoun,
                    Nid   = fke.Int(1, 1000),
                    Path  = fke.FolderPath
                });
            }

            return list;
        }


        protected override Func<SyncableFolderInfo, object> 
            GetKey => x => x.Nid;
    }
}
