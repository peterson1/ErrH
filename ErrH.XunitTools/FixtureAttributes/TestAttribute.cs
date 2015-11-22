using Xunit;

namespace ErrH.XunitTools.FixtureAttributes
{
    public class TestAttribute : TheoryAttribute
    {
        public TestAttribute(string displayName)
        {
            DisplayName = displayName;
        }
    }
}
