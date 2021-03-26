using IMT.DeveloperConnect.API.Client.Json;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace IMT.DeveloperConnect.API.Client.Http.Content
{
    public class ObjectContent<T> : HttpContent
        {
            private readonly T _value;

            public ObjectContent(T value)
            {
                _value = value;
                Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
            {
                return Task.Run(() =>
                {
                    using (JsonTextWriter jsonTextWriter = new JsonTextWriter(new StreamWriter(stream, Encoding.UTF8)) { CloseOutput = false })
                    {
                        JsonSerializer jsonSerializer = JsonSerializer.Create(JsonConstants.JsonSerializerSettings);
                        jsonSerializer.Serialize(jsonTextWriter, _value);
                        jsonTextWriter.Flush();
                    }
                });
            }

            protected override bool TryComputeLength(out long length)
            {
                length = -1;
                return false;
            }
        }
}
