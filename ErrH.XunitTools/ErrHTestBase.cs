using ErrH.JsonNetShim;
using ErrH.Tools.Extensions;
using ErrH.Tools.Randomizers;
using ErrH.Tools.Serialization;
using Xunit.Abstractions;

namespace ErrH.XunitTools
{
    public abstract class ErrHTestBase
    {
        protected FakeFactory       Fake   { get; } = new FakeFactory();
        protected ISerializer       Json   { get; } = new JsonNetSerializer();
        protected ITestOutputHelper StdOut { get; }


        public ErrHTestBase(ITestOutputHelper helpr)
        {
            MustExtensions.OutputHelper = StdOut = helpr;
        }


        protected void Log(string text) => StdOut.WriteLine(text);

        protected void H1(string text)
        {
            Log("");
            Log("= ".Repeat(20));
            Log("");
            Log(text);
            Log("");
            Log("= ".Repeat(20));
            Log("");
        }

    }
}
