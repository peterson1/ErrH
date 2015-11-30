using System;
using ErrH.Tools.CollectionShims;
using ErrH.XunitTools;
using ErrH.XunitTools.FixtureAttributes;
using Xunit;
using Xunit.Abstractions;

namespace ErrH.Tools.Tests.CollectionShims.DailyTxn1Key_Tests
{
    public class DailyTxn1Key_Facts : ErrHTestBase
    {
        public DailyTxn1Key_Facts(ITestOutputHelper helpr) : base(helpr)
        {
        }



        [Try("DailyTxn1Key: Allocate 1 month", Skip = "optimize memory consumption later")]
        public void OneMonth()
        {
            var start = new DateTime(2013, 1, 1);
            var end   = new DateTime(2013, 1, 31);
            var keys  = new int[] { 101, 102, 103 };
            var sut   = new DailyTxn1Key<TestStruct1>();

            sut.AllocateMemory(keys, start, end);

            sut.Data.MustNotBeNull("sut.Data");
            sut.Data.Length.MustBe(1, "years count");
            sut.Data[0].Length.MustBe(1, "months in year[0]");
            sut.Data[0][0].Length.MustBe(31, "days in month[0] year[0]");

            for (int i = 0; i < sut.Data[0][0].Length; i++)
            {
                sut.Data[0][0][i].Length.MustBe(keys.Length, 
                    $"keys in day[{i}] month[0] year[0]");

                for (int j = 0; j < keys.Length; j++)
                    sut.Data[0][0][i][j].MustBe(null, 
                        $"key[{j}] of day[{i}] month[0] year[0]");
            }
        }


        [Try("DailyTxn1Key: Allocate 1.5 years", Skip = "optimize memory consumption later")]
        public void Contructor()
        {
            var start = new DateTime(2013, 7, 1);
            var end   = new DateTime(2014, 12, 31);
            var keys = new int[] { 101, 102, 103 };
            var sut = new DailyTxn1Key<TestStruct1>();

            sut.AllocateMemory(keys, start, end);

            sut.Data.MustNotBeNull("sut.Data");
            sut.Data.Length.MustBe(2, "years count");

            sut.Data[0].Length.MustBe(6, "months in year[0]");
            sut.Data[1].Length.MustBe(12, "months in year[1]");
        }


        [Try("DailyTxn1Key")]
        public void Case1()
        {
            var start = new DateTime(2013, 1, 1);
            var keys  = new int[] { 101, 102, 103 };
            var sut   = new DailyTxn1Key<TestStruct1>();

            sut.AllocateMemory(keys, start);

            TestStruct1 s;
            s.Field1 = 123;
            s.Field2 = 456;

            sut[2013, 10, 15, 101] = s;

            sut[2013, 10, 15, 101].Field1.MustBe(123);

            Assert.Throws(typeof(IndexOutOfRangeException), () =>
            {
                sut[2013, 10, 15, 102].Field2.MustBe(888);
            });
        }




        private struct TestStruct1
        {
            public int Field1;
            public int Field2;
        }

        private class TestClass1
        {
            public int Field1 { get; set; }
            public int Field2 { get; set; }
        }
    }
}
