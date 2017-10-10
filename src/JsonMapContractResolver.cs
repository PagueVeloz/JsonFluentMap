using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace JsonFluentMap
{
    public class JsonMapContractResolver<T> : DefaultContractResolver
    {
        private readonly JsonMap<T> _map;

        public JsonMapContractResolver(JsonMap<T> map)
        {
            _map = map;
        }

        internal JsonProperty CreateProperty(MemberInfo member)
        {
            return base.CreateProperty(member, MemberSerialization.OptOut);
        }

        protected override IList<JsonProperty> CreateProperties(
            Type type
            , MemberSerialization memberSerialization
        )
        {
            var props = _map.HasType(type)
                ? _map.BuildProperties(type)
                : new List<JsonProperty>(0);

            return props;
        }
    }
}
