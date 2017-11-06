using System;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace JsonFluentMap
{
    /// <summary>
    /// The class for mapping a property
    /// </summary>
    public class JsonPropertyMap
    {
        [NotNull] private readonly JsonProperty _jsonProp;

        internal JsonPropertyMap([NotNull] JsonProperty jsonProp)
        {
            _jsonProp = jsonProp;
        }

        /// <summary>
        /// Ignore this property
        /// </summary>
        public void Ignore()
        {
            _jsonProp.Ignored = true;
        }

        /// <summary>
        /// Changes the name of this property
        /// </summary>
        /// <param name="name">The name of this property</param>
        /// <exception cref="ArgumentNullException">Will be thrown when <paramref name="name"/> is null</exception>
        public JsonPropertyMap WithName([NotNull] string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            _jsonProp.PropertyName = name;
            return this;
        }

        /// <summary>
        /// Changes the converter for this property
        /// </summary>
        /// <param name="converter">A json converter</param>
        public JsonPropertyMap ConvertWith(JsonConverter converter)
        {
            _jsonProp.Converter = converter;
            return this;
        }

        /// <summary>
        /// Changes the default value for this property
        /// </summary>
        public JsonPropertyMap DefaultValue(object defaultValue)
        {
            _jsonProp.DefaultValue = defaultValue;
            return this;
        }

        /// <summary>
        /// Changes the order for this property
        /// </summary>
        /// <remarks>
        /// If one property is using order, the rest must use too
        /// </remarks>
        public JsonPropertyMap Order(int? order)
        {
            _jsonProp.Order = order;
            return this;
        }

        internal JsonProperty Build()
        {
            return _jsonProp;
        }
    }
}