using IMT.DeveloperConnect.API.Client.Json;
using Newtonsoft.Json;
using System.Net.Http;

namespace IMT.DeveloperConnect.API.Client.Http.Content
{
    public class ObjectContentBuilder<T> : IContentBuilder
    {
        private readonly T _value;
        public ObjectContentBuilder(T value)
        {
            _value = value;
        }

        public HttpContent Build()
        {
            return new ObjectContent<T>(_value);
        }

        public string GetPayloadForLogging()
        {
            return JsonConvert.SerializeObject(_value, Formatting.Indented, JsonConstants.JsonSerializerSettings);
        }
    }
}
