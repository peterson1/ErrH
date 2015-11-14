using Xunit;
using Xunit.Abstractions;

namespace ErrH.XunitTools
{

    [Collection("ErrTestBase")]
    public abstract class ErrTestBase
    {
        public ErrTestBase(ITestOutputHelper helpr)
        {
            MustExtensions.OutputHelper = helpr;
        }
    }
}
