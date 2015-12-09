using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrH.Tools.CollectionShims;
using ErrH.XunitTools;
using ErrH.XunitTools.FixtureAttributes;
using Xunit;
using Xunit.Abstractions;

namespace ErrH.Tools.Tests.CollectionShims.DailyList_Tests
{
    public class DailyList_NoDuplicates : ErrHTestBase
    {
        public DailyList_NoDuplicates(ITestOutputHelper helpr) : base(helpr)
        {
        }


        [Try("DailyList : no duplicates")]
        public void NoDups()
        {
            var sut = new DailyList<TestStruct>();
            sut.AllocateMemory(new DateTime(2015, 12, 1));

            var s1 = new TestStruct(1, 2, 3, 4);
            var s2 = new TestStruct(1, 2, 3, 4);

            Assert.Null(sut[2015, 12, 1]);

            sut[2015, 12, 1] = new HashSet<TestStruct>();
            sut[2015, 12, 1].Add(s1).MustBe(true);
            sut[2015, 12, 1].Add(s2).MustBe(false);

            sut[2015, 12, 1].Count.MustBe(1);
        }



        private struct TestStruct
        {
            public readonly int     Int1;
            public readonly int     Int2;
            public readonly decimal Dec1;
            public readonly decimal Dec2;


            public TestStruct(int i1, int i2, decimal d1, decimal d2)
            {
                Int1 = i1;
                Int2 = i2;
                Dec1 = d1;
                Dec2 = d2;
            }
        }
    }
}
