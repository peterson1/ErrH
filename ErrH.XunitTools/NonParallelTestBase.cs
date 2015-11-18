using Xunit;
using Xunit.Abstractions;

namespace ErrH.XunitTools
{

    [Collection("NonParallelTests")]
    public abstract class NonParallelTestBase : ErrHTestBase
    {

        public NonParallelTestBase(ITestOutputHelper helpr) 
            : base(helpr)
        {
        }
    }
}
