using System;
using System.Collections.Generic;
using ErrH.Tools.CollectionShims;
using Xunit;

namespace ErrH.Tools.Tests.CollectionShims.ListRepoBase_Facts
{
    public class Has_Facts
    {
        [Fact(DisplayName="Handles null")]
        public void Nulls()
        {
            var strRepo = new StringKeyRepo();
            var numRepo = new NumberKeyRepo();

            Assert.False(strRepo.Has(null));
            Assert.False(numRepo.Has(null));

            strRepo.Add(new TestClass("a"));
            numRepo.Add(new TestClass(1));

            Assert.False(strRepo.Has(null));
            Assert.False(numRepo.Has(null));
        }


        [Fact(DisplayName = "returns True/False")]
        public void TrueFalse()
        {
            var strRepo = new StringKeyRepo();
            var numRepo = new NumberKeyRepo();

            strRepo.Add(new TestClass("a"));
            numRepo.Add(new TestClass(1)  );

            Assert.True(strRepo.Has(new TestClass("a")));
            Assert.True(numRepo.Has(new TestClass(1)));

            Assert.False(strRepo.Has(new TestClass("b")));
            Assert.False(numRepo.Has(new TestClass(2)));
        }



        private class NumberKeyRepo : ListRepoBase<TestClass>
        {
            protected override Func<TestClass, object>
                GetKey => x => x.Id;

            protected override List<TestClass> LoadList(object[] args) => null;
        }


        private class StringKeyRepo : ListRepoBase<TestClass>
        {
            protected override Func<TestClass, object> 
                GetKey => x => x.Name;

            protected override List<TestClass> LoadList(object[] args) => null;
        }


        private class TestClass
        {
            public int     Id    { get; set; }
            public string  Name  { get; set; }

            public TestClass(int id) { Id = id; }
            public TestClass(string nme) { Name = nme; }
        }
    }
}
