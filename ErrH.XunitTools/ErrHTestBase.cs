using ErrH.JsonNetShim;
using ErrH.Tools.Randomizers;
using ErrH.Tools.Serialization;
using Xunit.Abstractions;

namespace ErrH.XunitTools
{
    public abstract class ErrHTestBase
    {
        protected FakeFactory Fake { get; } = new FakeFactory();
        protected ISerializer Json { get; } = new JsonNetSerializer();


        public ErrHTestBase(ITestOutputHelper helpr)
        {
            MustExtensions.OutputHelper = helpr;
        }
    }
}
