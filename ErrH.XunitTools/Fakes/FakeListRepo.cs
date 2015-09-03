using System;
using System.Collections.Generic;
using System.Linq;
using ErrH.Tools.CollectionShims;
using Ploeh.AutoFixture;

namespace ErrH.XunitTools.Fakes
{
    public class FakeListRepo<T> : ListRepoBase<T>
    {
        public FakeListRepo(int itemCount)
            : base(Build<T>(itemCount))
        {
        }


        private static List<T> Build<T1>(int count) where T1 : T
            => new Fixture().CreateMany<T>(count).ToList();


        protected override Func<T, object> 
            GetKey => x => x.ToString();


        protected override List<T> 
            LoadList(object[] args) => _list;
    }
}
