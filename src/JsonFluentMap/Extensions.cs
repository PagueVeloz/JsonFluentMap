using System;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace JsonFluentMap
{
    /// <summary>
    /// Extensions for the JsonFluentMap stuff
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Generates a new <see cref="JsonSerializer"/> based on <paramref name="settings"/> or <see cref="JsonMapSettings.BaseSettings"/>
        /// </summary>
        /// <param name="contractResolver">The contract resolver to be put in the <see cref="JsonSerializer"/></param>
        /// <param name="settings">Settings used to create the JsonSerializer</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static JsonSerializer BuildSerializer(
            [NotNull] this IContractResolver contractResolver,
            JsonSerializerSettings settings = null
        )
        {
            if (contractResolver == null)
                throw new ArgumentNullException(nameof(contractResolver));

            if (settings == null)
                settings = JsonMapSettings.BaseSettings;

            var serializer = new JsonSerializer
            {
                CheckAdditionalContent = settings.CheckAdditionalContent,
                ConstructorHandling = settings.ConstructorHandling,
                Context = settings.Context,
                ContractResolver = contractResolver,
                Culture = settings.Culture,
                DateFormatHandling = settings.DateFormatHandling,
                DateFormatString = settings.DateFormatString,
                DateParseHandling = settings.DateParseHandling,
                DateTimeZoneHandling = settings.DateTimeZoneHandling,
                DefaultValueHandling = settings.DefaultValueHandling,
                EqualityComparer = settings.EqualityComparer,
                FloatFormatHandling = settings.FloatFormatHandling,
                FloatParseHandling = settings.FloatParseHandling,
                Formatting = settings.Formatting,
                MaxDepth = settings.MaxDepth,
                MetadataPropertyHandling = settings.MetadataPropertyHandling,
                MissingMemberHandling = settings.MissingMemberHandling,
                NullValueHandling = settings.NullValueHandling,
                ObjectCreationHandling = settings.ObjectCreationHandling,
                PreserveReferencesHandling = settings.PreserveReferencesHandling,
                ReferenceLoopHandling = settings.ReferenceLoopHandling,
                StringEscapeHandling = settings.StringEscapeHandling,
                TraceWriter = settings.TraceWriter,
                TypeNameAssemblyFormatHandling = settings.TypeNameAssemblyFormatHandling,
                TypeNameHandling = settings.TypeNameHandling
            };

            if (settings.SerializationBinder != null)
                serializer.SerializationBinder = settings.SerializationBinder;

            var refResolver = settings.ReferenceResolverProvider?.Invoke();

            if (refResolver != null)
                serializer.ReferenceResolver = refResolver;

            return serializer;
        }
    }
}