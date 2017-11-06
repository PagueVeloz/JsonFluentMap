using System;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace JsonFluentMap
{
    /// <summary>
    /// The base settings
    /// </summary>
    public static class JsonMapSettings
    {
        /// <summary>
        /// Base settings
        /// </summary>
        public static JsonSerializerSettings BaseSettings { get; set; }
            = new JsonSerializerSettings();
    }
}