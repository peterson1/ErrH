using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit2;

namespace ErrH.Tools.Tests.DataAttributes
{
    public class FakeDataAttribute : AutoDataAttribute
    {
        public FakeDataAttribute()
        {
            var c = new SupportMutableValueTypesCustomization();
            c.Customize(Fixture);
        }


    }
}
