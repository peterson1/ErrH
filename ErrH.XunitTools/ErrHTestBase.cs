using ErrH.Tools.Randomizers;
using Xunit.Abstractions;

namespace ErrH.XunitTools
{
    public abstract class ErrHTestBase
    {
        protected FakeFactory Fake { get; } = new FakeFactory();


        public ErrHTestBase(ITestOutputHelper helpr)
        {
            MustExtensions.OutputHelper = helpr;
        }
    }
}
