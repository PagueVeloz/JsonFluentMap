using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace JsonFluentMap
{
    /// <summary>
    /// The contract resolver for the <see cref="JsonMap{T}"/>
    /// </summary>
    /// <typeparam name="T">Type of the mapped model</typeparam>
    public class JsonMapContractResolver<T> : DefaultContractResolver
    {
        private readonly JsonMap<T> _map;

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="map">The json map that will be used to resolve the contract</param>
        /// <exception cref="ArgumentNullException">Will be thrown when <paramref name="map"/> is null</exception>
        public JsonMapContractResolver([NotNull] JsonMap<T> map)
        {
            _map = map ?? throw new ArgumentNullException(nameof(map));
        }

        internal JsonProperty CreateProperty(MemberInfo member)
        {
            return base.CreateProperty(member, MemberSerialization.OptOut);
        }

        /// <inheritdoc />
        protected override IList<JsonProperty> CreateProperties(
            Type type
            , MemberSerialization memberSerialization
        )
            => _map.BuildProperties(type);
    }
}
