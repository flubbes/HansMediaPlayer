using Newtonsoft.Json;
using RestSharp.Serializers;
using System.IO;

namespace Hans.Services
{
    /// <summary>
    /// The warpper to fuckup the restsharp serializer and use newtonsoft. BAMZ
    /// </summary>
    public class RestSharpJsonSerializeWrapper : ISerializer
    {
        private readonly Newtonsoft.Json.JsonSerializer _serializer;

        /// <summary>
        /// Initializes a new instance of the RestSharpJsonSerializeWrapper
        /// </summary>
        public RestSharpJsonSerializeWrapper()
        {
            ContentType = "application/json";
            _serializer = new Newtonsoft.Json.JsonSerializer
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Include,
                DefaultValueHandling = DefaultValueHandling.Include
            };
        }

        /// <summary>
        /// The content type
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// THe dateformat
        /// </summary>
        public string DateFormat { get; set; }

        /// <summary>
        /// THe namespace
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// The root element
        /// </summary>
        public string RootElement { get; set; }

        /// <summary>
        /// Serializes an object to json
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string Serialize(object obj)
        {
            using (var jsonTextWriter = new JsonTextWriter(new StringWriter()))
            {
                jsonTextWriter.Formatting = Formatting.Indented;
                jsonTextWriter.QuoteChar = '"';

                _serializer.Serialize(jsonTextWriter, obj);

                var result = new StringWriter().ToString();
                return result;
            }
        }
    }
}