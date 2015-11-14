using Xunit;
using Xunit.Abstractions;

namespace ErrH.XunitTools
{

    [Collection("NonParallelTests")]
    public abstract class NonParallelTestBase
    {
        public NonParallelTestBase(ITestOutputHelper helpr)
        {
            MustExtensions.OutputHelper = helpr;
        }
    }
}
