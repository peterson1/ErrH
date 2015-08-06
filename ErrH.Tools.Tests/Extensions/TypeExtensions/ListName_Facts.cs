using System;
using System.Collections.Generic;
using ErrH.Tools.Extensions;
using Xunit;

namespace ErrH.Tools.Tests.Extensions.TypeExtensions
{
    public class ListName_Facts
    {
        [Theory]
        [InlineData(typeof(int[]), "Int32[]")]
        [InlineData(typeof(decimal[]), "Decimal[]")]
        [InlineData(typeof(List<string>), "List‹String›")]
        //[InlineData(typeof(Dictionary<int, decimal>), "Dictionary<Int32, Decimal>")]//todo: support Dictionary
        public void TestSomething(Type typ, string expctd)
        {
            Assert.Equal(expctd, typ.ListName());
        }

    }
}
