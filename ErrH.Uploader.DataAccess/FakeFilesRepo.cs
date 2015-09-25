using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.Extensions;
using ErrH.Tools.FileSynchronization;
using ErrH.Tools.Randomizers;

namespace ErrH.Uploader.DataAccess
{
    public class FakeFilesRepo : ListRepoBase<SyncableFileRemote>
    {
        public override Task<bool> LoadAsync(params object[] args)
        {
            _list = LoadList(args);
            return true.ToTask();
        }


        protected override List<SyncableFileRemote> LoadList(object[] args)
        {
            var fke = new FakeFactory();
            var list = new List<SyncableFileRemote>();

            //later: FakeFactory method for: SHA1
            //later: FakeFactory method for: version info
            for (int i = 0; i < fke.Int(5, 10); i++)
            {
                list.Add(new SyncableFileRemote
                {
                    Name = fke.Filename,
                    Fid = fke.Int(1, 1000),
                    Nid = fke.Int(1, 1000),
                    SHA1 = fke.Word.SHA1(),
                    Size = fke.Int(1000, int.MaxValue),
                    Version = fke.Word,
                    Vid = fke.Int(1, 1000),
                });
            }

            return list;
        }


        protected override Func<SyncableFileRemote, object>
            GetKey => x => x.Nid;
    }
}
