using Newtonsoft.Json;
using Xunit;

namespace JsonFluentMap.Tests
{
    public class JsonMapSettingsTests
    {
        [Fact]
        public void BaseSettingsGetterAndSetterAreAcessible()
        {
            var foo = JsonMapSettings.BaseSettings;
            Assert.NotNull(foo);

            var settings = new JsonSerializerSettings();

            JsonMapSettings.BaseSettings = settings;
            Assert.Same(settings, JsonMapSettings.BaseSettings);
        }
    }
}