using Xunit;

namespace ErrH.XunitTools.FixtureAttributes
{
    public class TryAttribute : FactAttribute
    {

        public TryAttribute(string displayName)
        {
            DisplayName = displayName;
        }
    }
}
