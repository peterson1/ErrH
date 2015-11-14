using ErrH.Tools.SqlHelpers;
using ErrH.XunitTools;
using Xunit;
using Xunit.Abstractions;

namespace ErrH.Tools.Tests.SqlHelpers
{
    public class DbRowMapper_Override : ErrTestBase
    {
        public DbRowMapper_Override(ITestOutputHelper helpr) 
            : base(helpr) { }


        [Fact(DisplayName = "Map Override string")]
        public void Case1()
        {
            var row = new ResultRow();
            row.Add("FirstName", "abc");
            row.Add("LastName", "_d-e-f_");

            var expctd = new SampleClass1
            {
                FirstName = "abc",
                LastName = "def",
            };
            var actual = new SampleClass1();
            var mapOvr = new SampleClass1_MapOverride();
            DbRowMapper.Map(row, actual, mapOvr).MustBe(true, "Map() ret val");
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


        private class SampleClass1_MapOverride : MapOverrideBase
        {
            public SampleClass1_MapOverride()
            {
                Register(nameof(SampleClass1.LastName), OVerrideLastName);
            }

            private object OVerrideLastName(object origName)
            {
                var s = origName.ToString().Replace("_", "");
                return s.Replace("-", "");
            }
        }
    }
}
