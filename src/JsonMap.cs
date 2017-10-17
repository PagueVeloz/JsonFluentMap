using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Newtonsoft.Json.Serialization;

namespace JsonFluentMap
{
    /// <summary>
    /// Entry class for mapping models
    /// </summary>
    /// <typeparam name="T">Type of the model</typeparam>
    public abstract class JsonMap<T>
    {
        private readonly Dictionary<Type, List<JsonPropertyMap>> _properties
            = new Dictionary<Type, List<JsonPropertyMap>>();

        private readonly JsonMapContractResolver<T> _jsonMapContractResolver;

        /// <summary>
        /// The constructor
        /// </summary>
        protected JsonMap()
        {
            _jsonMapContractResolver = new JsonMapContractResolver<T>(this);
        }

        /// <summary>
        /// Add a new property to the model map
        /// </summary>
        /// <param name="expression">Property selector</param>
        /// <param name="name">Name of the property in json</param>
        /// <typeparam name="TProp">Type of the property</typeparam>
        /// <returns>The property map</returns>
        /// <exception cref="ArgumentNullException">Will be thrown in case that any parameters are null</exception>
        /// <exception cref="ArgumentException">Will be thrown when the expression is not a <see cref="MemberExpression"/></exception>
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

            if (_properties.ContainsKey(declaringType))
                _properties[declaringType].Add(prop);
            else
                _properties.Add(declaringType, new List<JsonPropertyMap> {prop});

            return prop;
        }

        /// <summary>
        /// Add a map for a nested type
        /// </summary>
        /// <typeparam name="TSub">Type of the nested type</typeparam>
        /// <typeparam name="TMap">Type of the map for the nested type</typeparam>
        /// <exception cref="InvalidOperationException">Will be thrown when trying to add a submap for one type that already has a map</exception>
        public void AddSubMap<TSub, TMap>()
            where TMap : JsonMap<TSub>
        {
            AddSubMap<TSub, TMap>(Activator.CreateInstance<TMap>);
        }

        /// <summary>
        /// Add a map for a nested type
        /// </summary>
        /// <param name="submapCtor">Activator for the submap</param>
        /// <typeparam name="TSub">Type of the nested type</typeparam>
        /// <typeparam name="TMap">Type of the map for the nested type</typeparam>
        /// <exception cref="InvalidOperationException">Will be thrown when trying to add a submap for one type that already has a map</exception>
        /// <exception cref="ArgumentNullException">Will be thrown when <paramref name="submapCtor"/> is null</exception>
        public void AddSubMap<TSub, TMap>([NotNull] Func<TMap> submapCtor)
            where TMap : JsonMap<TSub>
        {
            if (submapCtor == null)
                throw new ArgumentNullException(nameof(submapCtor));

            //TODO: Create a map cache
            var subMap = submapCtor();

            foreach (var kv in subMap._properties)
            {
                if (kv.Key == typeof(T))
                    continue;

                if (_properties.ContainsKey(kv.Key))
                {
                    throw new InvalidOperationException();
                }

                _properties.Add(kv.Key, kv.Value);
            }
        }

        internal bool HasType(Type type)
        {
            return _properties.ContainsKey(type);
        }

        internal IList<JsonProperty> BuildProperties([NotNull] Type type)
        {
            var props = _properties[type]
                .Select(p => p.Build())
                .ToList();

            return props;
        }

        /// <summary>
        /// Generates the contract resolver of this map
        /// </summary>
        /// <returns>The contract resolver for this map</returns>
        public IContractResolver BuildContractResolver()
        {
            return _jsonMapContractResolver;
        }
    }
}
