using System;
using Xunit;

namespace JsonFluentMap.Tests
{
    public class JsonMapContractResolverTests
    {
        [Fact]
        public void CtorChecksForNulls()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            var argEx = Assert.Throws<ArgumentNullException>(() => new JsonMapContractResolver<int>(null));
            Assert.Equal("map", argEx.ParamName);
        }
    }
}