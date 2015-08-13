using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrH.Tools.Extensions;
using Xunit;

namespace ErrH.Tools.Tests.Extensions.CollectionExtensions
{
    public class Has_Facts
    {
        [Fact(DisplayName= "string[]: null array")]
        public void StringArrayNull()
        {
            string[] sut = null;
            Assert.False(sut.Has(x => x == "abc"));
        }


        [Fact(DisplayName = "string[]: no match")]
        public void StringArrayNoMatch()
        {
            string[] sut = { "a", "b", "c" };
            Assert.False(sut.Has(x => x == "d"));
        }


        [Fact(DisplayName = "string[]: Happy! :-)")]
        public void StringArrayHappy()
        {
            string[] sut = { "a", "b", "c" };
            Assert.True(sut.Has(x => x == "b"));
        }


        [Fact(DisplayName = "List<T>: no match")]
        public void List_T_NoMatch()
        {
            var sut = new List<SampleType>
            {
                new SampleType { Text = "a" },
                new SampleType { Text = "b" },
                new SampleType { Text = "c" },
            };
            Assert.False(sut.Has(x => x.Text == "d"));
        }


        [Fact(DisplayName = "List<T>:: Happy! :-)")]
        public void List_T_Happy()
        {
            var sut = new List<SampleType>
            {
                new SampleType { Text = "a" },
                new SampleType { Text = "b" },
                new SampleType { Text = "c" },
            };
            Assert.True(sut.Has(x => x.Text == "b"));
        }



        private class SampleType
        {
            public string Text { get; set; }
        }
    }
}
