using ErrH.Tools.Randomizers;
using Xunit;
using Xunit.Abstractions;

namespace ErrH.XunitTools
{

    [Collection("NonParallelTests")]
    public abstract class NonParallelTestBase
    {
        protected FakeFactory Fake { get; } = new FakeFactory();


        public NonParallelTestBase(ITestOutputHelper helpr)
        {
            MustExtensions.OutputHelper = helpr;
        }
    }
}
