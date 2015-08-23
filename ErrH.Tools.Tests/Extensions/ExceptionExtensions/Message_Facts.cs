using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrH.Tools.ErrorConstructors;
using ErrH.Tools.Extensions;
using Xunit;

namespace ErrH.Tools.Tests.Extensions.ExceptionExtensions
{
    public class Message_Facts
    {
        [Fact(DisplayName = "Error In Cast")]
        public void testName()
        {
            Assert.Throws<InvalidCastException>(() => {
                Throw.BadCast<Array>("a");
            });
        }


        [Fact(DisplayName = "Zero InnerExceptions")]
        public void test0()
        {
            var sut = new Exception("Main Error");

            var expctd = "« Main Error »";

            Assert.Equal(expctd, sut.Details(false, false));
        }

        [Fact(DisplayName = "1 InnerException")]
        public void test1()
        {
            var inr = new Exception("InnerEx");
            var sut = new Exception("Main Error", inr);

            var expctd = "« Main Error »"
                 + L.f + ". InnerEx"
                 ;

            Assert.Equal(expctd, sut.Details(false, false));
        }

        [Fact(DisplayName = "3 InnerExceptions")]
        public void test2()
        {
            var err3 = new Exception("Err line 3");
            var err2 = new Exception("Err line 2", err3);
            var err1 = new Exception("Err line 1", err2);
            var sut = new Exception("Main Error", err1);

            var expctd = "« Main Error »"
                 + L.f + ". Err line 1"
                 + L.f + ".. Err line 2"
                 + L.f + "... Err line 3"
                 ;

            Assert.Equal(expctd, sut.Details(false, false));
        }


    }
}
