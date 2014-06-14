using Newtonsoft.Json;
using RestSharp.Serializers;
using System.IO;

namespace Hans.Services
{
    public class RestSharpJsonSerializeWrapper : ISerializer
    {
        private readonly Newtonsoft.Json.JsonSerializer _serializer;

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

        public string ContentType { get; set; }

        public string DateFormat { get; set; }

        public string Namespace { get; set; }

        public string RootElement { get; set; }

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