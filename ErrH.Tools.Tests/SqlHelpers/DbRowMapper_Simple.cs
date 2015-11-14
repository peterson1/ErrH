using ErrH.Tools.SqlHelpers;
using ErrH.XunitTools;
using Xunit;
using Xunit.Abstractions;

namespace ErrH.Tools.Tests.SqlHelpers
{
    public class DbRowMapper_Simple : NonParallelTestBase
    {
        public DbRowMapper_Simple(ITestOutputHelper helpr) 
            : base(helpr) { }



        [Fact(DisplayName = "Simple Map")]
        public void Case1()
        {
            var row = new ResultRow();
            row.Add("FirstName", "abc");
            row.Add("LastName", "def");

            var expctd = new SampleClass1
            {
                FirstName = "abc",
                LastName  = "def",
            };
            var actual = new SampleClass1();
            DbRowMapper.Map(row, actual).MustBe(true, "Map() ret val");
            actual.MustBe(expctd);
        }


        [Fact(DisplayName = "Simple Map w/ inherit")]
        public void Case2()
        {
            var row = new ResultRow();
            row.Add("FirstName", "abc");
            row.Add("LastName" , "def");
            row.Add("ExtraCol" , "ghi");

            var expctd = new SampleClass2
            {
                FirstName = "abc",
                LastName = "def",
                ExtraCol = "ghi"
            };
            var actual = new SampleClass2();
            DbRowMapper.Map(row, actual).MustBe(true, "Map() ret val");
            actual.MustBe(expctd);
        }


        [DbTable("Tbl1", "recID")]
        private class SampleClass1
        {
            [DbCol("FName")]
            public string FirstName { get; set; }

            [DbCol("LName")]
            public string LastName { get; set; }
        }


        [DbTable("Tbl2", "recID2")]
        private class SampleClass2 : SampleBaseClass
        {
            [DbCol("FName")]
            public string FirstName { get; set; }

            [DbCol("LName")]
            public string LastName { get; set; }
        }

        private class SampleBaseClass
        {
            [DbCol("Extra")]
            public string ExtraCol { get; set; }
        }
    }
}
