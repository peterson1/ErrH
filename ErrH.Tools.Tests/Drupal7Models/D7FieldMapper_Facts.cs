using ErrH.Tools.Drupal7Models;
using ErrH.Tools.Drupal7Models.Entities;
using ErrH.Tools.Drupal7Models.FieldAttributes;
using ErrH.Tools.Drupal7Models.Fields;
using ErrH.Tools.Drupal7Models.FieldValues;
using ErrH.XunitTools;
using Xunit;
using Xunit.Abstractions;

namespace ErrH.Tools.Tests.Drupal7Models
{
    public class D7FieldMapper_Facts : ErrHTestBase
    {
        public D7FieldMapper_Facts(ITestOutputHelper helpr) : base(helpr)
        {
        }


        [Fact(DisplayName = "D7FieldMapper: Supports value2")]
        public void TestValue2()
        {
            var prop1  = Fake.Word;
            var prop2a = Fake.Word;
            var prop2b = Fake.Word;

            var input = new TestClass1
            {
                Prop1 = prop1,
                Prop2a = prop2a,
                Prop2b = prop2b
            };

            var expctd = new TestClass1D7
            {
                field_prop1 = und.Values(prop1),
                field_prop2 = und.Value1_2(prop2a, prop2b)
            };

            D7FieldMapper.Map(input).MustBe(expctd);

            //DbRowMapper.Map()
        }

        [D7NodeDto("testclass1", typeof(TestClass1D7))]
        private class TestClass1
        {
            [D7ValueField("field_prop1")]
            public string  Prop1  { get; set; }

            [D7TwoValueField1("field_prop2")]
            public string  Prop2a { get; set; }

            [D7TwoValueField2("field_prop2")]
            public string  Prop2b { get; set; }
        }

        private class TestClass1D7 : D7NodeBase
        {
            public FieldUnd<UndValue>   field_prop1 { get; set; }
            public FieldUnd<Und2Values> field_prop2 { get; set; }
        }
    }
}
