using System;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace JsonFluentMap
{
    public class JsonPropertyMap
    {
        [NotNull] private readonly JsonProperty _jsonProp;

        internal JsonPropertyMap([NotNull] JsonProperty jsonProp)
        {
            _jsonProp = jsonProp;
        }

        public void Ignore()
        {
            _jsonProp.Ignored = true;
        }

        public JsonPropertyMap WithName([NotNull] string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            _jsonProp.PropertyName = name;
            return this;
        }

        public JsonPropertyMap ConvertWith(JsonConverter converter)
        {
            _jsonProp.Converter = converter;
            return this;
        }

        internal JsonProperty Build()
        {
            return _jsonProp;
        }
    }
}
