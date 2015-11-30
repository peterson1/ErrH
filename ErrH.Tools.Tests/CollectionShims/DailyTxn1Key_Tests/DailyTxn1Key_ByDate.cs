using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.Tests.DataAttributes;
using ErrH.XunitTools;
using ErrH.XunitTools.FixtureAttributes;
using Ploeh.AutoFixture.Xunit2;
using Xunit;
using Xunit.Abstractions;

namespace ErrH.Tools.Tests.CollectionShims.DailyTxn1Key_Tests
{
    public class DailyTxn1Key_ByDate : ErrHTestBase
    {
        public DailyTxn1Key_ByDate(ITestOutputHelper helpr) : base(helpr)
        {
            
        }



        [Test("ByDate"), FakeData]
        public void Case1(DailyTxn1Key<TestStruct1>  sut, 
                          int[] k,
                          TestStruct1[] s)
        {
            sut.AllocateMemory(k, new DateTime(2013, 10, 15));

            sut[2013, 10, 15, k[0]] = s[0];
            sut[2013, 10, 15, k[1]] = s[1];
            sut[2013, 10, 16, k[2]] = s[2];

            sut[2013, 10, 15].MustNotBeNull();
            //sut[2013, 10, 15].
            sut[2013, 10, 15].Count().MustBe(2);

            sut[2013, 10, 16].MustNotBeNull();
            sut[2013, 10, 16].Count().MustBe(1);

            sut[2013, 10, 17].MustNotBeNull();
            sut[2013, 10, 17].Count().MustBe(0);
        }




        public struct TestStruct1
        {
            public int Field1;
            public int Field2;
        }
    }
}
