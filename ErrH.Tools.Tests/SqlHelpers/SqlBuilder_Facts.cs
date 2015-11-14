using System;
using ErrH.Tools.SqlHelpers;
using ErrH.XunitTools;
using Xunit;
using Xunit.Abstractions;

namespace ErrH.Tools.Tests.SqlHelpers
{
    public class SqlBuilder_Facts : ErrTestBase
    {
        public SqlBuilder_Facts(ITestOutputHelper helpr) 
            : base(helpr) { }


        [Fact(DisplayName = "SELECT")]
        public void SELECT()
        {
            var expctd = "SELECT FName AS FirstName," 
                             + " LName AS LastName" 
                        + " FROM Tbl1" 
                       + " ORDER BY recID;";
            SqlBuilder
                .SELECT<SampleClass1>()
                .MustBe(expctd);
        }


        [Fact(DisplayName = "SELECT by key")]
        public void SELECT_ByKey()
        {
            var expctd = "SELECT FName AS FirstName,"
                             + " LName AS LastName"
                        + " FROM Tbl1"
                       + " WHERE recID = 1234"
                       + " ORDER BY recID;";
            SqlBuilder
                .SELECT_ByKey<SampleClass1>(1234)
                .MustBe(expctd); ;
        }


        [Fact(DisplayName = "SELECT w/ inherited")]
        public void SELECT_w_inherited()
        {
            var expctd = "SELECT FName AS FirstName,"
                             + " LName AS LastName,"
                             + " Extra AS ExtraCol"
                        + " FROM Tbl2"
                       + " ORDER BY recID2;";
            SqlBuilder
                .SELECT<SampleClass2>()
                .MustBe(expctd);
        }




        [Fact(DisplayName = "“AS” not allowed in col name")]
        public void AS_not_Allowed()
        {
            Assert.Throws(typeof(ArgumentException), () =>
            {
                SqlBuilder.SELECT<SampleClass3>();
            });
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


        [DbTable("Tbl3", "recID3")]
        private class SampleClass3
        {
            [DbCol("FName")]
            public string FirstName { get; set; }

            [DbCol("LName AS LastN")]
            public string LastName { get; set; }
        }
    }
}
