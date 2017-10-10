using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Newtonsoft.Json.Serialization;

namespace JsonFluentMap
{
    public class JsonMap<T>
    {
        internal readonly Dictionary<Type, List<JsonPropertyMap>> Properties =
            new Dictionary<Type, List<JsonPropertyMap>>();

        private readonly JsonMapContractResolver<T> _jsonMapContractResolver;

        public JsonMap()
        {
            _jsonMapContractResolver = new JsonMapContractResolver<T>(this);
        }

        public JsonPropertyMap AddProperty<TProp>(
            [NotNull] Expression<Func<T, TProp>> expression
            , [NotNull] string name
        )
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            var lambdaExpression = (LambdaExpression) expression;

            if (!(lambdaExpression.Body is MemberExpression memberExpression))
                throw new ArgumentException(nameof(lambdaExpression));

            var propInfo = (PropertyInfo) memberExpression.Member;
            var declaringType = propInfo.DeclaringType;
            var jsonProp = _jsonMapContractResolver.CreateProperty(propInfo);
            var prop = new JsonPropertyMap(jsonProp)
                .WithName(name);

            if (Properties.ContainsKey(declaringType))
                Properties[declaringType].Add(prop);
            else
                Properties.Add(declaringType, new List<JsonPropertyMap> {prop});

            return prop;
        }

        public void AddSubMap<TSub, TMap>()
            where TMap : JsonMap<TSub>
        {
            //TODO: Create a map cache
            var subMap = Activator.CreateInstance<TMap>();

            foreach (var kv in subMap.Properties)
            {
                if (kv.Key == typeof(T))
                    continue;

                if (Properties.ContainsKey(kv.Key))
                {
                    throw new InvalidOperationException();
                }

                Properties.Add(kv.Key, kv.Value);
            }
        }

        internal bool HasType(Type type)
        {
            return Properties.ContainsKey(type);
        }

        internal IList<JsonProperty> BuildProperties([NotNull] Type type)
        {
            var props = Properties[type]
                .Select(p => p.Build())
                .ToList();

            return props;
        }

        public IContractResolver BuildContractResolver()
        {
            return _jsonMapContractResolver;
        }
    }
}
