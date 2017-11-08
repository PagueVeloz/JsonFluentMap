using System;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace JsonFluentMap.Tests
{
    public class ExtensionsTests
    {
        private class ExtensionsFixture : Fixture
        {
            public ExtensionsFixture()
            {
                this.Register<IContractResolver>(() => new DefaultContractResolver());
                this.Register<JsonConverter>(() => null);
                this.Register<IEqualityComparer>(() => null);
                this.Register<IReferenceResolver>(() => null);
                this.Register<ITraceWriter>(() => null);
                //this should be removed in the next versions of newtonsoft.json
                this.Register<SerializationBinder>(() => null);
                this.Register<ISerializationBinder>(() => null);
            }
        }

        private class AutoDataExtensionsFixtureAttribute : AutoDataAttribute
        {
            public AutoDataExtensionsFixtureAttribute()
                : base(new ExtensionsFixture())
            {
            }
        }

        [Theory, AutoDataExtensionsFixture]
        public void CanGenerateSerializerWithBaseSettings(IContractResolver contractResolver)
        {
            var serializer = contractResolver.BuildSerializer();

            Assert.NotNull(serializer);
            Assert.Same(contractResolver, serializer.ContractResolver);
        }

        [Theory, AutoDataExtensionsFixture]
        public void CanGenerateSerializerWithMySettings(
            IContractResolver contractResolver,
            JsonSerializerSettings mySettings
        )
        {
            var serializer = contractResolver.BuildSerializer(mySettings);

            Assert.NotNull(serializer);
            Assert.Same(contractResolver, serializer.ContractResolver);

            Assert.NotSame(mySettings.ContractResolver, serializer.ContractResolver);

            Assert.Equal(mySettings.CheckAdditionalContent, serializer.CheckAdditionalContent);
            Assert.Equal(mySettings.ConstructorHandling, serializer.ConstructorHandling);
            Assert.Equal(mySettings.Context, serializer.Context);
            Assert.Equal(mySettings.Culture, serializer.Culture);
            Assert.Equal(mySettings.DateFormatHandling, serializer.DateFormatHandling);
            Assert.Equal(mySettings.DateFormatString, serializer.DateFormatString);
            Assert.Equal(mySettings.DateParseHandling, serializer.DateParseHandling);
            Assert.Equal(mySettings.DateTimeZoneHandling, serializer.DateTimeZoneHandling);
            Assert.Equal(mySettings.DefaultValueHandling, serializer.DefaultValueHandling);
            Assert.Equal(mySettings.EqualityComparer, serializer.EqualityComparer);
            Assert.Equal(mySettings.FloatFormatHandling, serializer.FloatFormatHandling);
            Assert.Equal(mySettings.FloatParseHandling, serializer.FloatParseHandling);
            Assert.Equal(mySettings.Formatting, serializer.Formatting);
            Assert.Equal(mySettings.MaxDepth, serializer.MaxDepth);
            Assert.Equal(mySettings.MetadataPropertyHandling, serializer.MetadataPropertyHandling);
            Assert.Equal(mySettings.MissingMemberHandling, serializer.MissingMemberHandling);
            Assert.Equal(mySettings.NullValueHandling, serializer.NullValueHandling);
            Assert.Equal(mySettings.ObjectCreationHandling, serializer.ObjectCreationHandling);
            Assert.Equal(mySettings.PreserveReferencesHandling, serializer.PreserveReferencesHandling);
            Assert.Equal(mySettings.ReferenceLoopHandling, serializer.ReferenceLoopHandling);
            Assert.Equal(mySettings.StringEscapeHandling, serializer.StringEscapeHandling);
            Assert.Equal(mySettings.TraceWriter, serializer.TraceWriter);
            Assert.Equal(mySettings.TypeNameAssemblyFormatHandling, serializer.TypeNameAssemblyFormatHandling);
            Assert.Equal(mySettings.TypeNameHandling, serializer.TypeNameHandling);
        }

        [Fact]
        public void BuildSerializerThrowsArgumentNull()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => ((IContractResolver) null).BuildSerializer());
        }
    }
}