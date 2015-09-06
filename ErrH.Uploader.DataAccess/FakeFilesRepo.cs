using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.Extensions;
using ErrH.Tools.Randomizers;
using ErrH.Uploader.Core.Nodes;

namespace ErrH.Uploader.DataAccess
{
    public class FakeFilesRepo : ListRepoBase<AppFileNode>
    {
        public override Task<bool> LoadAsync(params object[] args)
        {
            _list = LoadList(args);
            return true.ToTask();
        }


        protected override List<AppFileNode> LoadList(object[] args)
        {
            var fke = new FakeFactory();
            var list = new List<AppFileNode>();

            //later: FakeFactory method for: SHA1
            //later: FakeFactory method for: version info
            for (int i = 0; i < fke.Int(5, 10); i++)
            {
                list.Add(new AppFileNode
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


        protected override Func<AppFileNode, object>
            GetKey => x => x.Nid;
    }
}
