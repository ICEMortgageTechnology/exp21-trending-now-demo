using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace IMT.DeveloperConnect.API.Client.Http.Content
{
    public class FileContent : HttpContent
    {
        private readonly string _filePath;

        public FileContent(string filePath, string contentType)
        {
            _filePath = filePath;
            if (string.IsNullOrEmpty(contentType))
                contentType = "application/octet-stream";
            Headers.ContentType = new MediaTypeHeaderValue(contentType);
        }

        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            using (Stream fs = new FileStream(_filePath, FileMode.Open, FileAccess.Read))
            {
                await fs.CopyToAsync(stream);
            }
        }

        protected override bool TryComputeLength(out long length)
        {
            length = new FileInfo(_filePath).Length;
            return true;
        }
    }
}
