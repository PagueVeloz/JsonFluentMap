using System.IO;
using System.Text;
using Newtonsoft.Json;
using Xunit;

namespace JsonFluentMap.Tests
{
    public class JsonMapTests
    {
        [Fact]
        public void CanMapSuperClassProperties()
        {
            var contractResolver = new AnotherDummyClass.JsonMap()
                .BuildContractResolver();
            var serializer = new JsonSerializer
            {
                ContractResolver = contractResolver
            };

            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            using (var jsonText = new JsonTextWriter(sw))
            {
                serializer.Serialize(jsonText, new AnotherDummyClass
                {
                    Test = "Foo",
                    Test2 = "Bar"
                });
            }

            Assert.Equal("{\"anotherTest\":\"Bar\",\"test\":\"Foo\"}", sb.ToString());
        }

        private class DummyClass
        {
            public string Test { get; set; }

            public sealed class JsonMap : JsonMap<DummyClass>
            {
                public JsonMap()
                {
                    AddProperty(d => d.Test, "test");
                }
            }
        }

        private class AnotherDummyClass : DummyClass
        {
            public string Test2 { get; set; }

            public new class JsonMap : JsonMap<AnotherDummyClass>
            {
                public JsonMap()
                {
                    AddProperty(d => d.Test2, "anotherTest");

                    AddSuperMap<DummyClass, DummyClass.JsonMap>();
                }
            }
        }
    }
}